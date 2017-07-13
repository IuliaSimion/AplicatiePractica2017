using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class AnnouncementCreateModel
    {
        [Required]
        public string Phonenumber { get; set; }
        [Required]
        public string Email { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}