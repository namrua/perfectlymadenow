using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Base.Contract.Identities;
using AutomationSystem.Main.Contract.Registrations.AppLogic;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Classes.System.Extensions;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.Data.Models;
using AutomationSystem.Main.Core.Registrations.System;
using AutomationSystem.Main.Model;
using System;

namespace AutomationSystem.Main.Core.Registrations.AppLogic
{
    public class RegistrationPersonalDataAdministration : IRegistrationPersonalDataAdministration
    {
        private readonly IClassDatabaseLayer classDb;
        private readonly IIdentityResolver identityResolver;
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IRegistrationLogicProvider registrationLogicProvider;
        private readonly IRegistrationPersonalDataService dataService;

        public RegistrationPersonalDataAdministration(
            IClassDatabaseLayer classDb,
            IIdentityResolver identityResolver,
            IRegistrationDatabaseLayer registrationDb,
            IRegistrationLogicProvider registrationLogicProvider,
            IRegistrationPersonalDataService dataService)
        {
            this.classDb = classDb;
            this.identityResolver = identityResolver;
            this.registrationDb = registrationDb;
            this.registrationLogicProvider = registrationLogicProvider;
            this.dataService = dataService;
        }

        public RegistrationControllerInfo GetControllerInfoByRegistrationTypeId(RegistrationTypeEnum registrationTypeId)
        {
            // note: no identity check needed here

            var convertor = registrationLogicProvider.GetConvertorByRegistrationTypeId(registrationTypeId);
            var result = convertor.ControllerInfo;
            return result;
        }
        
        public BaseRegistrationDetail GetRegistrationDetailById(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.AddressesCountry |
                ClassRegistrationIncludes.RegistrationType | ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registration.RegistrationTypeId);
            var result = service.GetRegistrationDetail(registration);
            
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEdit(
            RegistrationTypeEnum registrationTypeId, long classId)
        {
            var cls = GetClassById(classId);
            identityResolver.CheckEntitleForClass(cls);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registrationTypeId);
            var result = service.GetNewRegistrationForEdit(registrationTypeId, cls);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetRegistrationForEdit(long registrationId)
        {
            var registration = GetRegistrationById(registrationId, ClassRegistrationIncludes.Addresses | ClassRegistrationIncludes.Class);
            identityResolver.CheckEntitleForClass(registration.Class);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(registration.RegistrationTypeId);
            var result = service.GetRegistrationForEdit(registration);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetFormRegistrationForEdit(BaseRegistrationForm form)
        {
            var toCheck = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(toCheck);

            var service = registrationLogicProvider.GetServiceByRegistrationTypeId(form.RegistrationTypeId);
            var result = service.GetFormRegistrationForEdit(form);
            return result;
        }
        
        public long SaveRegistration(BaseRegistrationForm form)
        {
            var toCheck = GetClassById(form.ClassId);
            identityResolver.CheckEntitleForClass(toCheck);
            var result = dataService.SaveRegistration(form);
            return result;
        }

        #region private methods
        private Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class with id {classId}.");
            }
                
            return result;
        }
        
        private ClassRegistration GetRegistrationById(long registrationId, ClassRegistrationIncludes includes = ClassRegistrationIncludes.None)
        {
            var result = registrationDb.GetClassRegistrationById(registrationId, includes);
            if (result == null)
            {
                throw new ArgumentException($"There is no Class registration with id {registrationId}.");
            }
               
            return result;
        }
        #endregion

    }
}
