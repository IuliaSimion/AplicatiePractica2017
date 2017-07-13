using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Review : ReviewCreateModel
    {
        public int ReviewId { get; set; }
        public DateTime DatePosted { get; set; }

    }
}