using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Contact
    {
        [Key]
        public long ContactID { get; set; }
        public long? SiteID { get; set; }
        public DateTime? DATE { get; set; }
        public string? NAME { get; set; }
        public string? CONTACT { get; set; }
        public string? CHILD { get; set; }
        public string? SITUATION { get; set; }
    }
}
