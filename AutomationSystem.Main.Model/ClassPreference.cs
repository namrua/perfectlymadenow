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
    
    public partial class ClassPreference
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClassPreference()
        {
            this.ClassPreferenceExpenses = new HashSet<ClassPreferenceExpense>();
            this.Profiles = new HashSet<Profile>();
        }
    
        public long ClassPreferenceId { get; set; }
        public string HomepageUrl { get; set; }
        public Nullable<long> HeaderPictureId { get; set; }
        public AutomationSystem.Base.Contract.Enums.RegistrationColorSchemeEnum RegistrationColorSchemeId { get; set; }
        public string VenueName { get; set; }
        public string LocationCode { get; set; }
        public bool SendCertificatesByEmail { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> Changed { get; set; }
        public bool Deleted { get; set; }
        public AutomationSystem.Base.Contract.Enums.CurrencyEnum CurrencyId { get; set; }
        public Nullable<long> LocationInfoId { get; set; }
    
        public virtual Currency Currency { get; set; }
        public virtual Person LocationInfo { get; set; }
        public virtual RegistrationColorScheme RegistrationColorScheme { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassPreferenceExpense> ClassPreferenceExpenses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Profile> Profiles { get; set; }
    }
}