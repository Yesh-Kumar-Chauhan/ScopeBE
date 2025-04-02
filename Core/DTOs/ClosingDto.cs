using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ClosingDto
    {
        public long ClosingID { get; set; }
        public long? DistrictID { get; set; }
        public DateTime? DATE { get; set; }
        public long? STATUS { get; set; }
        public string? StatusName { get; set; }
        public bool ParentCredit { get; set; }
        public bool STAFF_PH { get; set; }
        public bool STAFF_DT { get; set; }
        public bool StaffPaid { get; set; }
        public string? NOTES { get; set; }
        public bool MakeUpDay { get; set; }
    }
}
