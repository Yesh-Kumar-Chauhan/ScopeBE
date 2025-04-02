using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Timesheet
    {
        [Key]
        public long TimesheetID { get; set; }
        public long? DistrictID { get; set; }
        public long? SiteID { get; set; }
        public long? SchoolID { get; set; }
        public long? PersonID { get; set; }
        public string? Position { get; set; }
        public DateTime? TimeSheetDate { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public TimeSpan? LunchIn { get; set; }
        public TimeSpan? LunchOut { get; set; }
        public TimeSpan? AdditionalStart { get; set; }
        public TimeSpan? AdditionalStop { get; set; }
        public string? DeviceID { get; set; }
        public int? Type { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ClockInLocal { get; set; }
        public DateTime? ClockOutLocal { get; set; }
        public string? NotesHeader { get; set; }
        public string? NotesDetails { get; set; }

        public TimeSpan? SiteTimeIn { get; set; }
        public TimeSpan? SiteTimeOut { get; set; }
        public long? ExternalEventId { get; set; }

        public string? Paycode { get; set; }

        public string? Code { get; set; }
    }
}
