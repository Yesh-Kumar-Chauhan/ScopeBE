using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Visit
    {
        [Key]
        public long VisitID { get; set; }
        public long? SiteID { get; set; }
        public DateTime? DATE { get; set; }
        public string? NAME { get; set; }
        public string? TIMEIN { get; set; }
        public string? TIMEOUT { get; set; }
        public string? NOTES { get; set; }
        public bool? OFFICAL { get; set; }
        public bool? STAFFING { get; set; }
        public bool? PROBLEM { get; set; }
        public bool? TRAINING { get; set; }
        public bool? QUALITY { get; set; }
        public bool? OTHER { get; set; }
    }
}
