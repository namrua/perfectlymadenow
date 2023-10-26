using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.FormerClasses.AppLogic.Models.FormerClasses
{
    /// <summary>
    /// Former class filter
    /// </summary>
    [Bind(Include = "ClassTypeId, Location, FromDate, ToDate")]
    public class FormerClassFilter
    {

        [DisplayName("Type")]
        [PickInputOptions(NoItemText = "all types", Placeholder = "all types")]
        public ClassTypeEnum? ClassTypeId { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("From date")]
        public DateTime? FromDate { get; set; }

        [DisplayName("To date")]
        public DateTime? ToDate { get; set; }

        public long? ProfileId { get; set; }


        // if not null, synonymous class types is used instead of ClassTypeId
        public HashSet<ClassTypeEnum> SynonymousClassTypes;

    }

}
