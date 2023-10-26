using System;
using System.Collections.Generic;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.Base;
using AutomationSystem.Main.Core.Certificates.System.Convertors;
using AutomationSystem.Main.Core.Certificates.System.Models;
using AutomationSystem.Main.Core.Classes.AppLogic.Models;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.FileServices.System;
using AutomationSystem.Main.Core.FileServices.System.Models;
using AutomationSystem.Main.Core.Persons.Data;
using AutomationSystem.Main.Core.Persons.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Main.Model;
using PerfectlyMadeInc.Helpers.Database;

namespace AutomationSystem.Main.Core.Certificates.System
{
    public class CertificateService : ICertificateService
    {
        private const string wordSuffix = ".docx";
        private const int registrantParticipantMaxLength = 30;

        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IPersonDatabaseLayer personDb;
        private readonly IMainFileService mainFileService;
        private readonly ICertificateConvertor certificateConvertor;
        private readonly ICertificateDocumentCreator certificateCreator;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        private readonly IClassTypeResolver classTypeResolver;

        private readonly Dictionary<ClassTypeTopic, string> certificatesDefinitionMap;
        private readonly Dictionary<ClassTypeTopic, string> wwaCertificatesDefinitionMap;
        private readonly Dictionary<ClassTypeTopic, string> twoLineWwaCertificatesDefinitionMap;

        public CertificateService(
            IRegistrationDatabaseLayer registrationDb,
            IMainFileService mainFileService,
            IClassDatabaseLayer classDb,
            IPersonDatabaseLayer personDb,
            ICertificateConvertor certificateConvertor,
            ICertificateDocumentCreator certificateCreator,
            IRegistrationTypeResolver registrationTypeResolver,
            IClassTypeResolver classTypeResolver)
        {
            this.registrationDb = registrationDb;
            this.mainFileService = mainFileService;
            this.classDb = classDb;
            this.personDb = personDb;
            this.certificateConvertor = certificateConvertor;
            this.certificateCreator = certificateCreator;
            this.registrationTypeResolver = registrationTypeResolver;
            this.classTypeResolver = classTypeResolver;

            twoLineWwaCertificatesDefinitionMap = new Dictionary<ClassTypeTopic, string>();

            certificatesDefinitionMap = new Dictionary<ClassTypeTopic, string>
            {
                { ClassTypeTopic.Basic, "Certificate of Completion - Basic I"},
                { ClassTypeTopic.Basic2, "Certificate of Completion - Basic II"},
                { ClassTypeTopic.Business, "Certificate of Completion - Business"},
                { ClassTypeTopic.Health, "Certificate of Completion - Health"},
            };
            wwaCertificatesDefinitionMap = new Dictionary<ClassTypeTopic, string>
            {
                { ClassTypeTopic.Basic, "Certificate of Completion - WWA Basic I"},
                { ClassTypeTopic.Basic2, "Certificate of Completion - WWA Basic II"},
                { ClassTypeTopic.Business, "Certificate of Completion - WWA Business"},
                { ClassTypeTopic.Health, "Certificate of Completion - WWA Health"},
            };
            twoLineWwaCertificatesDefinitionMap = new Dictionary<ClassTypeTopic, string>
            {
                { ClassTypeTopic.Basic, "Certificate of Completion - TL WWA Basic I"},
                { ClassTypeTopic.Basic2, "Certificate of Completion - TL WWA Basic II"},
                { ClassTypeTopic.Business, "Certificate of Completion - TL WWA Business"},
                { ClassTypeTopic.Health, "Certificate of Completion - TL WWA Health"},
            };
        }

        public void GenerateCertificates(string rootPath, long classId)
        {
            // gets class
            var cls = classDb.GetClassById(classId, ClassIncludes.ClassPersons | ClassIncludes.ClassStyle);
            if (cls == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }

            // checks class type           
            if (!classTypeResolver.AreCertificatesAllowed(cls.ClassCategoryId))
            {
                throw new InvalidOperationException($"Generating of certificate is not allowed for class category {cls.ClassCategoryId}.");
            }

            // gets persons
            var persons = new List<Person>();
            var allowedForPersons = classTypeResolver.AreCertificatesAllowedForClassPersons(cls.ClassCategoryId);
            if (allowedForPersons)
            {
                var instructorStuffIds = new HashSet<long>();
                if (cls.GuestInstructorId.HasValue)
                    instructorStuffIds.Add(cls.GuestInstructorId.Value);
                instructorStuffIds.UnionWith(cls.ClassPersons
                    .Where(x => x.RoleTypeId == PersonRoleTypeEnum.ApprovedStaff ||
                                x.RoleTypeId == PersonRoleTypeEnum.Instructor)
                    .Select(x => x.PersonId));
                persons = personDb.GetPersonsByIds(instructorStuffIds, PersonIncludes.Address);
            }

            // gets registrations
            var filter = new RegistrationFilter { ClassId = classId, RegistrationState = RegistrationState.Approved };
            var registrations = registrationDb.GetRegistrationsByFilter(filter, ClassRegistrationIncludes.Addresses);

            // generate certificates
            registrations.ForEach(x => GenerateCertificateForRegistration(rootPath, cls, x));
            persons.ForEach(x => GenerateCertificateForPerson(rootPath, cls, x));

            // generates multi certificate
            if (!cls.ClassStyle.SendCertificatesByEmail && allowedForPersons)
            {
                var nonWwaRegistrations = registrations.Where(x => !registrationTypeResolver.IsWwaRegistration(x.RegistrationTypeId)).ToList();
                GenerateMultiCertificateForRegistrationsAndPersons(rootPath, cls, persons, nonWwaRegistrations);
            }
        }

        public void GenerateCertificate(string rootPath, long registrationId)
        {
            // gets registration
            var registration = registrationDb.GetClassRegistrationById(registrationId, ClassRegistrationIncludes.Addresses | ClassRegistrationIncludes.Class);
            if (registration == null)
            {
                throw new ArgumentException($"There is no registration with id {registrationId}.");
            }

            // checks class type           
            if (!classTypeResolver.AreCertificatesAllowed(registration.Class.ClassCategoryId))
            {
                throw new InvalidOperationException($"Generating of certificate is not allowed for class category {registration.Class.ClassCategoryId}.");
            }

            // generates certificate
            GenerateCertificateForRegistration(rootPath, registration.Class, registration);
        }

        #region word workflow

        private void GenerateCertificateForRegistration(string rootPath, Class cls, ClassRegistration registration)
        {
            // converts data, select template, generates certificate 
            var topic = classTypeResolver.GetClassTypeInfo(cls.ClassTypeId).Topic;
            var certificateInfo = certificateConvertor.ConvertToCertificateInfo(cls, registration);
            var templateFileName = GetTemplateFileNameByCertificateInfoAndTopic(certificateInfo, topic);
            var content = certificateCreator.CreateCertificateDocument(rootPath, templateFileName + wordSuffix, certificateInfo);

            // creates file to save object and save certificate to database
            var displayedName = DatabaseHelper.TrimNVarchar($"Certificate - {certificateInfo.Name}", true, 120);
            var fileToSave = new EntityFileToSave
            {
                EntityId = registration.ClassRegistrationId,
                EntityTypeId = EntityTypeEnum.MainClassRegistration,
                Code = MainFileReservedNames.Certificate,
                DisplayedName = displayedName,
                FileName = displayedName + wordSuffix,
                FileTypeId = FileTypeEnum.Word,
                Content = content
            };
            mainFileService.SaveClassRegistrationFile(fileToSave);
        }

        private string GetTemplateFileNameByCertificateInfoAndTopic(CertificateInfo certificateInfo, ClassTypeTopic topic)
        {
            // use normal template for non-WWA
            if (!certificateInfo.UseWwaTemplate)
            {
                return certificatesDefinitionMap[topic];
            }

            // use normal WWA template for Names lenght limit
            if (certificateInfo.RegistrantParticipant.Length <= registrantParticipantMaxLength)
            {
                return wwaCertificatesDefinitionMap[topic];
            }

            // otherwise use two-line WWA template
            return twoLineWwaCertificatesDefinitionMap[topic];
        }

        private void GenerateCertificateForPerson(string rootPath, Class cls, Person person)
        {
            var topic = classTypeResolver.GetClassTypeInfo(cls.ClassTypeId).Topic;
            var templateFileName = certificatesDefinitionMap[topic];

            // converts data, generate certificate
            var certificateInfo = certificateConvertor.ConvertToCertificateInfo(cls, person);
            var content = certificateCreator.CreateCertificateDocument(rootPath, templateFileName + wordSuffix, certificateInfo);

            // creates file to save object and save certificate to database
            var displayedName = DatabaseHelper.TrimNVarchar($"Certificate - {certificateInfo.Name}", true, 120);
            var fileToSave = new EntityFileToSave
            {
                EntityId = cls.ClassId,
                EntityTypeId = EntityTypeEnum.MainClass,
                Code = $"{MainFileReservedNames.Certificate}-{person.PersonId}",
                DisplayedName = displayedName,
                FileName = displayedName + wordSuffix,
                FileTypeId = FileTypeEnum.Word,
                Content = content
            };
            mainFileService.SaveClassFile(fileToSave);
        }

        private void GenerateMultiCertificateForRegistrationsAndPersons(string rootPath, Class cls, 
            List<Person> persons, List<ClassRegistration> registrations)
        {
            var topic = classTypeResolver.GetClassTypeInfo(cls.ClassTypeId).Topic;
            var templateFileName = certificatesDefinitionMap[topic];

            // converts data and generates certificate
            var certificatesInfo = new List<CertificateInfo>();
            certificatesInfo.AddRange(persons.Select(x => certificateConvertor.ConvertToCertificateInfo(cls, x)).OrderBy(x => x.Name));
            certificatesInfo.AddRange(registrations.Select(x => certificateConvertor.ConvertToCertificateInfo(cls, x)).OrderBy(x => x.Name));
            var content = certificateCreator.CreateMultiCertificateDocument(rootPath, templateFileName + wordSuffix, certificatesInfo);

            // creates file to save object and save certificate to database
            var displayedName = "Certificates for Students and Staffs";
            var fileToSave = new EntityFileToSave
            {
                EntityId = cls.ClassId,
                EntityTypeId = EntityTypeEnum.MainClass,
                Code = MainFileReservedNames.MultiCertificate,
                DisplayedName = displayedName,
                FileName = displayedName + wordSuffix,
                FileTypeId = FileTypeEnum.Word,
                Content = content
            };
            mainFileService.SaveClassFile(fileToSave);
        }

        #endregion
    }
}
