using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.PersonalData
{
    /// <summary>
    /// Registration for edit
    /// </summary>
    /// <typeparam name="TForm">Registration form type</typeparam>
    public class RegistrationForEdit<TForm> : IRegistrationForEdit<TForm>  where TForm : BaseRegistrationForm
    {
        public ClassCategoryEnum ClassCategoryId { get; set; }
        public List<IEnumItem> Countries { get; set; }
        public List<IEnumItem> Languages { get; set; }
        public TForm Form { get; set; }

        public RegistrationForEdit(TForm form)
        {
            Form = form;
            Countries = new List<IEnumItem>();
            Languages = new List<IEnumItem>();
        }
    }
}
