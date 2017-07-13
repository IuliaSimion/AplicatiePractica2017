using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Review
    {
        public int ReviewId { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
        public int AnnouncementId { get; set; }

        public virtual Announcement Announcement { get; set; }
    }
}
