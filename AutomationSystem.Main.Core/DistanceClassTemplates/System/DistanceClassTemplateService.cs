using System;
using System.Linq;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.DistanceClassTemplates.AppLogic.Models.Base;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using AutomationSystem.Main.Core.Certificates.System;
using AutomationSystem.Main.Core.Classes.AppLogic;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.System;
using AutomationSystem.Main.Core.Classes.System.Convertors;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data;
using AutomationSystem.Main.Core.DistanceClassTemplates.Data.Models;
using AutomationSystem.Main.Core.DistanceClassTemplates.System.Models;
using AutomationSystem.Main.Core.DistanceProfiles.Data;
using AutomationSystem.Main.Core.DistanceProfiles.Data.Models;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.System.Models;
using PerfectlyMadeInc.Helpers.Routines;

namespace AutomationSystem.Main.Core.DistanceClassTemplates.System
{
    public class DistanceClassTemplateService : IDistanceClassTemplateService
    {
        public const CurrencyEnum DistanceClassCurrency = LocalisationInfo.DefaultCurrency;

        private readonly IDistanceClassTemplateClassDatabaseLayer distanceTemplateClassDb;
        private readonly IDistanceClassTemplateDatabaseLayer distanceTemplateDb;
        private readonly IDistanceProfileDatabaseLayer distanceProfileDb;
        private readonly ILanguageTranslationProvider languageTranslationProvider;
        private readonly IClassService classService;
        private readonly IClassActionService classActionService;
        private readonly ICertificateService certificateService;
        private readonly IClassDatabaseLayer classDb;

        public DistanceClassTemplateService(
            IDistanceClassTemplateClassDatabaseLayer distanceTemplateClassDb,
            IDistanceClassTemplateDatabaseLayer distanceTemplateDb,
            IDistanceProfileDatabaseLayer distanceProfileDb,
            ILanguageTranslationProvider languageTranslationProvider,
            IClassService classService,
            IClassActionService classActionService,
            ICertificateService certificateService,
            IClassDatabaseLayer classDb)
        {
            this.distanceTemplateClassDb = distanceTemplateClassDb;
            this.distanceTemplateDb = distanceTemplateDb;
            this.distanceProfileDb = distanceProfileDb;
            this.languageTranslationProvider = languageTranslationProvider;
            this.classService = classService;
            this.classActionService = classActionService;
            this.certificateService = certificateService;
            this.classDb = classDb;
        }

        public void PopulateDistanceClassesForDistanceProfile(long distanceProfileId)
        {
            // loads distance profile
            var distanceProfile = distanceProfileDb.GetDistanceProfileById(distanceProfileId, DistanceProfileIncludes.ProfileClassPreference);
            if (distanceProfile == null)
            {
                throw new ArgumentException($"There is no distance profile with id {distanceProfileId}.");
            }

            // loads distance class templates to be populated
            var fromRegistrationEnd = DateTime.Today.AddDays(1);
            var filledTemplateIds = distanceTemplateClassDb.GetFilledDistanceTemplateIdsForDistanceProfileId(distanceProfileId, fromRegistrationEnd);
            var distanceTemplateFilter = new DistanceClassTemplateFilter
            {
                TemplateState = DistanceClassTemplateState.Approved,
                FromRegistrationEnd = fromRegistrationEnd,
                ExcludeIds = filledTemplateIds
            };
            var distanceTemplates = distanceTemplateDb.GetDistanceClassTemplatesByFilter(distanceTemplateFilter, DistanceClassTemplateIncludes.DistanceClassTemplatePersons);

            // populate distance classes
            foreach (var distanceTemplate in distanceTemplates)
            {
                CreateDistanceClass(distanceTemplate, distanceProfile);
            }
        }

        public void PopulateDistanceClassesForDistanceTemplate(long distanceClassTemplateId)
        {
            // loads distance template
            var distanceTemplate = distanceTemplateDb.GetDistanceClassTemplateById(distanceClassTemplateId, DistanceClassTemplateIncludes.DistanceClassTemplatePersons);
            if (distanceTemplate == null)
            {
                throw new ArgumentException($"There is no distance class template with id {distanceClassTemplateId}.");
            }

            // loads activated distance profiles
            var filledProfileIds = distanceTemplateClassDb.GetFilledDistanceProfileIdsForDistanceClassTemplateId(distanceClassTemplateId);
            var distanceProfileFilter = new DistanceProfileFilter
            {
                IsActive = true,
                ExcludeIds = filledProfileIds
            };
            var distanceProfiles = distanceProfileDb.GetDistanceProfilesByFilter(distanceProfileFilter, DistanceProfileIncludes.ProfileClassPreference);

            // populate distance classes
            foreach (var distanceProfile in distanceProfiles)
            {
                CreateDistanceClass(distanceTemplate, distanceProfile);
            }
        }

        public void PropagateChangesToDistanceClasses(DistanceClassTemplate distanceTemplate)
        {
            var classIds = distanceTemplateClassDb.GetFilledClassIdsByDistanceClassTemplateId(distanceTemplate.DistanceClassTemplateId);
            var classesWithClassForms = classService.GetClassesWithClassFormsByIds(classIds);

            foreach (var classWithClassForm in classesWithClassForms)
            {
                SetFormByDistanceTemplate(classWithClassForm.ClassForm, distanceTemplate);
                classService.UpdateClass(classWithClassForm.ClassForm, DistanceClassCurrency, classWithClassForm.Class, true);
            }
        }

        public DistanceClassTemplateCompletionResult CompleteDistanceClassTemplate(long distanceTemplateId, string certificateRootPath)
        {
            var distanceTemplate = distanceTemplateDb.GetDistanceClassTemplateById(distanceTemplateId, DistanceClassTemplateIncludes.DistanceClassTemplateClasses);
            if (distanceTemplate == null)
            {
                throw new ArgumentException($"There is no distance template with id {distanceTemplateId}");
            }

            var classIds = distanceTemplate.DistanceClassTemplateClasses.Select(x => x.ClassId).ToList();
            var classes = classDb.GetClassesByIds(classIds);

            var result = new DistanceClassTemplateCompletionResult(distanceTemplateId);
            foreach(var cls in classes)
            {
                var classId = cls.ClassId;
                try
                {
                    // skips completed and cancelled classes
                    if (ClassConvertor.IsTerminalState(cls))
                    {
                        result.SkippedClasses.Add(classId);
                        continue;
                    }

                    // generate certificates
                    certificateService.GenerateCertificates(certificateRootPath, classId);

                    // complete class
                    var actionId = classActionService.CreateClassAction(cls, ClassActionTypeEnum.Completion);
                    classActionService.ProcessClassAction(actionId);
                    result.CompletedClasses.Add(classId);
                }
                catch (Exception e)
                {
                    result.Exception = e;
                    result.CorruptedClassId = classId;
                    return result;
                }
            }

            distanceTemplateDb.CompleteDistanceClassTemplate(distanceTemplateId);

            result.IsSuccess = true;
            return result;
        }

        #region private methods

        private void CreateDistanceClass(DistanceClassTemplate distanceTemplate, DistanceProfile distanceProfile)
        {
            var form = classService.GetNewClassForm(distanceProfile.ProfileId, ClassCategoryEnum.DistanceClass, DistanceClassCurrency, new EmptyPersonHelper());
            SetFormByDistanceTemplate(form, distanceTemplate);
            SetFormByDistanceProfile(form, distanceProfile);

            var classId = classService.InsertClass(form, DistanceClassCurrency, distanceProfile.Profile.ClassPreference, null);
            distanceTemplateClassDb.InsertDistanceClassTemplateClass(distanceTemplate.DistanceClassTemplateId, distanceProfile.DistanceProfileId, classId);
        }

        private void SetFormByDistanceTemplate(ClassForm form, DistanceClassTemplate distanceTemplate)
        {
            EntityHelper.CheckForNull(distanceTemplate.DistanceClassTemplatePersons, "DistanceClassTemplatePersons", "DistanceClassTemplate");

            form.ClassTypeId = distanceTemplate.ClassTypeId;
            form.Location = distanceTemplate.Location;
            form.EventStart = distanceTemplate.EventStart;
            form.EventEnd = distanceTemplate.EventEnd;
            form.RegistrationStart = distanceTemplate.RegistrationStart;
            form.RegistrationEnd = distanceTemplate.RegistrationEnd;
            form.GuestInstructorId = distanceTemplate.GuestInstructorId;
            form.TranslationCode = languageTranslationProvider.GetTranslationCode(distanceTemplate.OriginLanguageId, distanceTemplate.TransLanguageId);
            form.InstructorIds = distanceTemplate.DistanceClassTemplatePersons
                .Where(x => x.RoleTypeId == PersonRoleTypeEnum.Instructor)
                .Select(x => x.PersonId).ToList();
        }

        private void SetFormByDistanceProfile(ClassForm form, DistanceProfile distanceProfile)
        {
            form.PriceListId = distanceProfile.PriceListId;
            form.CoordinatorId = distanceProfile.DistanceCoordinatorId;
            form.PayPalKeyId = distanceProfile.PayPalKeyId;
        }

        #endregion
    }
}
