using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AutomationSystem.Base.Contract.Enums;
using CorabeuControl.ModelMetadata;

namespace AutomationSystem.Main.Contract.PriceLists.AppLogic.Models
{
    /// <summary>
    /// Price list form
    /// </summary>
    public class PriceListForm
    {
        // public properties
        [HiddenInput]
        public long PriceListId { get; set; }

        [HiddenInput]
        public PriceListTypeEnum PriceListTypeId { get; set; }

        [DisplayName("Name")]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Currency")]
        [PickInputOptions]
        public CurrencyEnum CurrencyId { get; set; }
       
        [DisplayName("Adult")]
        [Range(0, 1000000)]
        public decimal? NewAdult { get; set; }
        
        [DisplayName("Week of class for adult")]
        [Range(0, 1000000)]
        public decimal? NewAdultWeekOfClass { get; set; }
 
        [DisplayName("Child")]
        [Range(0, 1000000)]
        public decimal? NewChild { get; set; }
       
        [DisplayName("Review adult")]
        [Range(0, 1000000)]
        public decimal? ReviewAdult { get; set; }
       
        [DisplayName("Review child")]
        [Range(0, 1000000)]
        public decimal? ReviewChild { get; set; }
        
        [DisplayName("WWA")]
        [Range(0, 1000000)]
        public decimal? WWA { get; set; }

        [DisplayName("Lecture registration")]
        [Range(0, 1000000)]
        public decimal? LectureRegistration { get; set; }

        [DisplayName("Material registration")]
        [Range(0, 1000000)]
        public decimal? MaterialRegistration { get; set; }
    }
}