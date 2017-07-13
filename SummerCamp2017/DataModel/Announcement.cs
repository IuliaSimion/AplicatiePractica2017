using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Announcement
    {
        public int AnnouncementId { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public System.DateTime PostDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public int CategoryId { get; set; }
        public bool Closed { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual Category Category { get; set; }
       
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
