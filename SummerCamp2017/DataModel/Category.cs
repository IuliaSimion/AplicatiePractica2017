using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
