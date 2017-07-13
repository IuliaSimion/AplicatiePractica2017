using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class ReviewsPerAnnouncement
    {
        public int CurrentAnnouncementId { get; set; }
        public decimal AverageRating { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}