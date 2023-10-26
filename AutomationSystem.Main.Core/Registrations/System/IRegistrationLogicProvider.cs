using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Addresses.AppLogic.Convertors;
using AutomationSystem.Main.Core.Registrations.System.Convertors;

namespace AutomationSystem.Main.Core.Registrations.System
{

    /// <summary>
    /// provides registration convertors and services by specified type
    /// </summary>
    public interface IRegistrationLogicProvider
    {
        
        IAddressConvertor AddressConvertor { get; }
        
        IBaseRegistrationConvertor BaseRegistrationConvertor { get; }
        
        IRegistrationTypeFeeder RegistrationTypeFeeder { get; }
        
        
        void RegisterRegistrationConvertor<TForm, TDetail>(ITypedRegistrationConvertor<TForm, TDetail> convertor) 
            where TForm : BaseRegistrationForm
            where TDetail : BaseRegistrationDetail;

        IRegistrationConvertor GetConvertorByRegistrationTypeId(RegistrationTypeEnum registrationTypeId);        
        
        IRegistrationService GetServiceByRegistrationTypeId(RegistrationTypeEnum registrationTypeId);        

    }
}
