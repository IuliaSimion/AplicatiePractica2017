using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class AnnouncementEditModel : AnnouncementCreateModel
    {
        public int AnnouncementId { get; set; }

    }
}