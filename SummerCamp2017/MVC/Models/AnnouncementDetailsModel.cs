using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class AnnouncementDetailsModel : AnnouncementEditModel
    {
        public System.DateTime PostDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public bool Closed { get; set; }
        public  Category Category { get; set; }
    }
}