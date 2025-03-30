using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDomoshkaEF
{
    public class Role
    {
        public int Id { get; set; }
        public string NameRole { get; set; }
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
