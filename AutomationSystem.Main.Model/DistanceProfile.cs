//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutomationSystem.Main.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DistanceProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DistanceProfile()
        {
            this.DistanceClassTemplateClasses = new HashSet<DistanceClassTemplateClass>();
        }
    
        public long DistanceProfileId { get; set; }
        public long ProfileId { get; set; }
        public long PriceListId { get; set; }
        public long DistanceCoordinatorId { get; set; }
        public long PayPalKeyId { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> Activated { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> Changed { get; set; }
        public bool Deleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DistanceClassTemplateClass> DistanceClassTemplateClasses { get; set; }
        public virtual Person DistanceCoordinator { get; set; }
        public virtual PriceList PriceList { get; set; }
        public virtual Profile Profile { get; set; }
    }
}