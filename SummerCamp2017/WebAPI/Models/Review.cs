//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int ReviewId { get; set; }
        public Nullable<decimal> Rating { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public int AnnouncementId { get; set; }
        public System.DateTime DatePosted { get; set; }
    
        public virtual Announcement Announcement { get; set; }
    }
}
