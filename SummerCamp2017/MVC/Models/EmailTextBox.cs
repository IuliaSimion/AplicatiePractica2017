using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class EmailTextBox
    {
        [Required]
        public string Email { get; set; }
    }
}