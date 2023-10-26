using System;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData;
using AutomationSystem.Main.Core.Classes.Data;
using AutomationSystem.Main.Core.Classes.Data.Models;
using AutomationSystem.Main.Core.Registrations.Data;
using AutomationSystem.Main.Core.Registrations.System.Convertors;
using AutomationSystem.Main.Model;
using AutomationSystem.Shared.Contract.Localisation.System.Models;

namespace AutomationSystem.Main.Core.Registrations.System
{

    /// <summary>
    /// Registration typed service
    /// </summary>
    public class TypedRegistrationService<TForm, TDetail> : ITypedRegistrationService<TForm, TDetail>
        where TForm : BaseRegistrationForm
        where TDetail : BaseRegistrationDetail
    {

        private readonly IRegistrationDatabaseLayer registrationDb;
        private readonly IClassDatabaseLayer classDb;
        private readonly ITypedRegistrationConvertor<TForm, TDetail> registrationConvertor;
        
        public TypedRegistrationService(IRegistrationDatabaseLayer registrationDb,
            IClassDatabaseLayer classDb, ITypedRegistrationConvertor<TForm, TDetail> registrationConvertor)
        {            
            this.registrationDb = registrationDb;
            this.classDb = classDb;
            this.registrationConvertor = registrationConvertor;
        }       

        #region object operations
        
        public BaseRegistrationDetail GetRegistrationDetail(ClassRegistration registration)
        {
            var result = GetRegistrationDetailTyped(registration);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetNewRegistrationForEdit(RegistrationTypeEnum registrationTypeId, Class cls)
        {
            var result = GetNewRegistrationForEditTyped(registrationTypeId, cls);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetRegistrationForEdit(ClassRegistration registration)
        {
            var result = GetRegistrationForEditTyped(registration);
            return result;
        }
        
        public IRegistrationForEdit<BaseRegistrationForm> GetFormRegistrationForEdit(BaseRegistrationForm form)
        {
            if (!(form is TForm typedForm))
                throw GetCastException(nameof(form), typeof(TForm), form.GetType());
            var result = GetFormRegistrationForEditTyped(typedForm);
            return result;
        }
        
        public long SaveRegistration(BaseRegistrationForm form, bool insertAsTeporary, ApprovementTypeEnum approvementType)
        {
            if (!(form is TForm typedForm))
                throw GetCastException(nameof(form), typeof(TForm), form.GetType());
            var result = SaveRegistrationTyped(typedForm, insertAsTeporary, approvementType);
            return result;
        }

        #endregion


        #region typed operations
        
        public TDetail GetRegistrationDetailTyped(ClassRegistration registration)
        {           
            var result = registrationConvertor.ConvertToRegistrationDetail(registration);
            return result;
        }
        
        public RegistrationForEdit<TForm> GetNewRegistrationForEditTyped(RegistrationTypeEnum registrationTypeId, Class cls)
        {           
            var result = registrationConvertor.InitializeRegistrationForEdit(cls);
            result.Form = registrationConvertor.InitializeRegistrationForm(registrationTypeId, cls.ClassId);
            if (!cls.TransLanguageId.HasValue)
                result.Form.LanguageId = LocalisationInfo.DefaultLanguage;
            return result;
        }   
        
        public RegistrationForEdit<TForm> GetRegistrationForEditTyped(ClassRegistration registration)
        {            
            var result = registrationConvertor.InitializeRegistrationForEdit(registration.Class);
            result.Form = registrationConvertor.ConvertToRegistrationForm(registration);
            return result;
        }
        
        public RegistrationForEdit<TForm> GetFormRegistrationForEditTyped(TForm form)
        {
            var cls = GetClassById(form.ClassId);
            var result = registrationConvertor.InitializeRegistrationForEdit(cls);
            result.Form = form;
            return result;
        }
        
        public long SaveRegistrationTyped(TForm form, bool insertAsTeporary, ApprovementTypeEnum approvementType)
        {
            var cls = GetClassById(form.ClassId);
            var dbRegistration = registrationConvertor.ConvertToClassRegistration(form);
            var result = dbRegistration.ClassRegistrationId;
            if (result == 0)
            {
                dbRegistration.IsTemporary = insertAsTeporary;
                dbRegistration.ApprovementTypeId = approvementType;
                dbRegistration.ClassRegistrationPayment = new ClassRegistrationPayment();
                dbRegistration.ProfileId = cls.ProfileId;
                result = registrationDb.InsertClassRegistration(dbRegistration);
            }
            else
            {
                registrationDb.UpdateClassRegistration(dbRegistration);
            }
            return result;
        }

        #endregion


        #region private methodss
        
        private Exception GetCastException(string name, Type expected, Type actual)
        {
            return new InvalidCastException($"Cannot cast {name} with type {actual} to {expected}.");
        }
        
        private Class GetClassById(long classId, ClassIncludes includes = ClassIncludes.None)
        {
            var result = classDb.GetClassById(classId, includes);
            if (result == null)
                throw new ArgumentException($"There is no Class with id {classId}.");
            return result;
        }

        #endregion

    }

}
