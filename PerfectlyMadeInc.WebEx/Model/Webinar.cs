//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PerfectlyMadeInc.WebEx.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Webinar
    {
        public long WebinarId { get; set; }
        public long AccountId { get; set; }
        public Nullable<int> EntityTypeId { get; set; }
        public Nullable<long> EntityId { get; set; }
        public string MeetingId { get; set; }
        public string Name { get; set; }
        public string WebLink { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Changed { get; set; }
        public Nullable<bool> Deleted { get; set; }
    
        public virtual UserAccount UserAccount { get; set; }
    }
}
