using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class ReviewCreateModel
    {
        public Nullable<decimal> Rating { get; set; }
        public string Comment { get; set; }
        [Required]
        public string Username { get; set; }
        public int AnnouncementId { get; set; }
    }
}