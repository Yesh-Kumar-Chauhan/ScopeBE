using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Personel
{
    public class AttendanceDto
    {
        public long? AbsenceID { get; set; }

        public long AttendanceID { get; set; }
        public long? StaffId { get; set; } // Renaming STAFF_ID to PersonID for better understanding
        public DateTime? Date { get; set; }
        public string? Reason { get; set; }
        public string? AbsenceReason { get; set; }
        public bool? Paid { get; set; }
        public bool? Charged { get; set; }
        public string? Fraction { get; set; }
        public long? SiteNumber { get; set; } // Renaming SITENUM to SiteNumber for better understanding
        public string? SiteName { get; set; } // Renaming SITENAM to SiteName for better understanding
        public long? ReasonID { get; set; }
        public long? AbsentID { get; set; }
        public string? AbsentName { get; set; }
        public decimal? Weight { get; set; }
        public string? ReasonName { get; set; } // This can be used for displaying the name of the reason if needed.
    }

    public class AttendanceFormDto
    {
        public long AttendanceID { get; set; }
        public long? StaffId { get; set; } // Renaming STAFF_ID to PersonID for better understanding
        public DateTime? Date { get; set; }
        public string? Reason { get; set; }
        public bool? Paid { get; set; }
        public bool? Charged { get; set; }
        public string? Fraction { get; set; }
        public long? SiteNumber { get; set; } // Renaming SITENUM to SiteNumber for better understanding
        public string? SiteName { get; set; } // Renaming SITENAM to SiteName for better understanding
        public long? ReasonID { get; set; }
        public string? Paycode { get; set; }
    }
}
