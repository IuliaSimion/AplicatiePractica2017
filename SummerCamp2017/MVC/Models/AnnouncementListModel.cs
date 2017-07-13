using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class AnnouncementListModel 
    {
        public int AnnouncementId { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        [Required]
        public string Email { get; set; }
        public bool Closed { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public  Category Category { get; set; }
    }
}