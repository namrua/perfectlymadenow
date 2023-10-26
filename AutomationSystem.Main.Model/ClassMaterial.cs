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
    
    public partial class ClassMaterial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClassMaterial()
        {
            this.ClassMaterialFiles = new HashSet<ClassMaterialFile>();
            this.ClassMaterialRecipients = new HashSet<ClassMaterialRecipient>();
        }
    
        public long ClassMaterialId { get; set; }
        public string CoordinatorPassword { get; set; }
        public Nullable<System.DateTime> AutomationLockTime { get; set; }
        public Nullable<System.DateTime> AutomationLockTimeUtc { get; set; }
        public bool IsUnlocked { get; set; }
        public Nullable<System.DateTime> Unlocked { get; set; }
        public bool IsLocked { get; set; }
        public Nullable<System.DateTime> Locked { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> Changed { get; set; }
        public bool Deleted { get; set; }
        public long ClassId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassMaterialFile> ClassMaterialFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassMaterialRecipient> ClassMaterialRecipients { get; set; }
        public virtual Class Class { get; set; }
    }
}