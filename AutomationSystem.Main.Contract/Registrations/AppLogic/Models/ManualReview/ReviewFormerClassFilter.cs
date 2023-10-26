using System;
using AutomationSystem.Base.Contract.Enums;

namespace AutomationSystem.Main.Contract.Registrations.AppLogic.Models.ManualReview
{
    /// <summary>
    /// ReviewFormerClassFilter encapsulates query string fields for pre-filtering former classes to picking valid former students
    /// </summary>
    public class ReviewFormerClassFilter
    {
        public ClassTypeEnum? ClassTypeId { get; set; }
        public DateTime? FromDate { get; set; }
    }
}
