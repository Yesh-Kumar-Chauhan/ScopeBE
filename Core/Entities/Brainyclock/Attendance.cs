using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Brainyclock
{
    public class BrainyclockAttendance
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public TimeSpan? ClockIn { get; set; }
        public TimeSpan? ClockOut { get; set; }
        public TimeSpan? LunchIn { get; set; }
        public TimeSpan? LunchOut { get; set; }
        public int Overtime { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? DepartmentId { get; set; }
        public int MarkAttendanceBy { get; set; }
        public int EventId { get; set; }
        public int SiteId { get; set; }
        public int DistNumber { get; set; }
    }
}
