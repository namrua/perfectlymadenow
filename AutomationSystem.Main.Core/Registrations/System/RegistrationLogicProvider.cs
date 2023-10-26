using System;
using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System.Convertors;
using AutomationSystem.Main.Core.Registrations.System.RegistrationTypeFeeders;
using AutomationSystem.Shared.Contract.Enums.Data;
using AutomationSystem.Shared.Contract.Localisation.System;

namespace AutomationSystem.Main.Core.Registrations.System
{

    /// <summary>
    /// provides registration convertors and services by specified type
    /// </summary>
    public class RegistrationLogicProvider : IRegistrationLogicProviderLocalised
    {
        
        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly IRegistrationTypeResolver registrationTypeResolver;
        
        private readonly IDictionary<RegistrationFormTypeEnum, IRegistrationConvertor> convertorMap;
        private readonly IDictionary<RegistrationFormTypeEnum, IRegistrationService> serviceMap;


        public RegistrationLogicProvider(
            IRegistrationDatabaseLayer registrationDb,
            IClassDatabaseLayer classDb,
            IEnumDatabaseLayer enumDb,
            IRegistrationTypeFeeder registrationTypeFeeder,
            IAddressConvertor addressConvertor,
            IRegistrationStateProvider registrationStateProvider,
            IRegistrationTypeResolver registrationTypeResolver,
            ILocalisationService localisationService = null)
        {
            this.registrationDb = registrationDb;
            this.classDb = classDb;
            this.registrationTypeResolver = registrationTypeResolver;
            convertorMap = new Dictionary<RegistrationFormTypeEnum, IRegistrationConvertor>();
            serviceMap = new Dictionary<RegistrationFormTypeEnum, IRegistrationService>();

            AddressConvertor = addressConvertor;
            BaseRegistrationConvertor = new BaseRegistrationConvertor(enumDb, registrationStateProvider, registrationTypeResolver, localisationService);
            RegistrationTypeFeeder = registrationTypeFeeder;
            
        }

        
        public IAddressConvertor AddressConvertor { get; }
        
        public IBaseRegistrationConvertor BaseRegistrationConvertor { get; }
        
        public IRegistrationTypeFeeder RegistrationTypeFeeder { get; }

        
        public void RegisterRegistrationConvertor<TForm, TDetail>(ITypedRegistrationConvertor<TForm, TDetail> convertor)
            where TForm : BaseRegistrationForm
            where TDetail : BaseRegistrationDetail
        {
            var formType = convertor.RegistrationFormType;
            convertorMap.Add(formType, convertor);
            
            var service = new TypedRegistrationService<TForm, TDetail>(registrationDb, classDb, convertor);
            serviceMap.Add(formType, service);
        }
        
        
        public IRegistrationConvertor GetConvertorByRegistrationTypeId(RegistrationTypeEnum registrationTypeId)
        {
            var registrationFormTypeId = registrationTypeResolver.GetRegistrationFormType(registrationTypeId);
            if (!convertorMap.TryGetValue(registrationFormTypeId, out var result))
                throw new ArgumentException($"Registration form type {registrationFormTypeId} is not supported.");
            return result;
        }
        
        public IRegistrationService GetServiceByRegistrationTypeId(RegistrationTypeEnum registrationTypeId)
        {
            var registrationFormTypeId = registrationTypeResolver.GetRegistrationFormType(registrationTypeId);
            if (!serviceMap.TryGetValue(registrationFormTypeId, out var result))
                throw new ArgumentException($"Registration form type {registrationFormTypeId} is not supported.");
            return result;
        }

    }

}
