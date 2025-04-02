using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Closing
    {
        [Key]
        public long ClosingID { get; set; }
        public long? DistrictID { get; set; }
        public DateTime? DATE { get; set; }
        public long? STATUS { get; set; }
        public bool PARENT_CR { get; set; }
        public bool STAFF_PH { get; set; }
        public bool STAFF_DT { get; set; }
        public bool STAFF_ALL { get; set; }
        public string? NOTES { get; set; }
        public bool MakeUpDay { get; set; }
    }
}
