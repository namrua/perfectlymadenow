using System.Collections.Generic;
using AutomationSystem.Base.Contract.Enums;
using AutomationSystem.Main.Contract.Persons.AppLogic;
using CorabeuControl.Components;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Class for edit
    /// </summary>
    public class ClassForEdit
    {
        public ClassForm Form { get; set; }
        public ClassFormConfiguration FormConfiguration { get; set; }
        public List<IEnumItem> ClassTypes { get; set; }
        public List<IEnumItem> TimeZones { get; set; }
        public IPersonHelper PersonHelper { get; set; }
        public List<DropDownItem> Translations { get; set; }
        public List<DropDownItem> PriceLists { get; set; }
        public List<DropDownItem> PayPalKeys { get; set; }
        public List<DropDownItem> IntegrationEntities { get; set; }

        public bool IsInconsistentClassAndPriceListType { get; set; }
        public ClassTypeEnum? ForbiddenClassTypeId { get; set; }
        public long? ForbiddenPriceListId { get; set; }
        public EnvironmentTypeEnum? Env { get; set; }
        public bool CanFullEditClass { get; set; }

        // constructor
        public ClassForEdit()
        {
            Form = new ClassForm();
            FormConfiguration = new ClassFormConfiguration();
            ClassTypes = new List<IEnumItem>();
            TimeZones = new List<IEnumItem>();
            PersonHelper = new EmptyPersonHelper();
            Translations = new List<DropDownItem>();
            PriceLists = new List<DropDownItem>();
            PayPalKeys = new List<DropDownItem>();
            IntegrationEntities = new List<DropDownItem>();
        }
    }
}