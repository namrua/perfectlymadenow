using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.Classes.AppLogic.Models.Base
{
    /// <summary>
    /// Class filter
    /// </summary>
    [Bind(Include = "ClassState, ClassCategoryId, ProfileId, Env")]
    public class ClassFilter
    {
        // works only when complex state filters are false
        [DisplayName("Class state")]
        [PickInputOptions(NoItemText = "Open classes", Placeholder = "Open classes")]
        public ClassState? ClassState { get; set; }

        [DisplayName("Class category")]
        [PickInputOptions(NoItemText = "Class and Lectures", Placeholder = "Class and Lectures")]
        public ClassCategoryEnum? ClassCategoryId { get; set; }

        [DisplayName("Profile")]
        [PickInputOptions(NoItemText = "no profile", Placeholder = "select profile")]
        public long? ProfileId { get; set; }

        public bool NoDetachedHomepage { get; set; }

        // complex state filters
        public bool OnlyInAfterRegistration { get; set; }
        public bool OpenAndCompleted { get; set; }

        public DateTime? EventEndUtcFrom { get; set; }
        public DateTime? EventEndUtcTo { get; set; }

        public CurrencyEnum? CurrencyId { get; set; }

        // identity management filtering
        public List<long> ProfileIds { get; set; }

        public bool? IsWwaAllowed { get; set; }

        // only for testing purposes, default filtering value is Production
        public EnvironmentTypeEnum? Env { get; set; }
    }
}