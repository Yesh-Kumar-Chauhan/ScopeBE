using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Brainyclock
{
    public class Employee
    {
        public int Id { get; set; } 
        public int? CompanyId { get; set; } 
        public string? DepartmentId { get; set; } 
        public string? CreatedBy { get; set; }
        public string? FName { get; set; } 
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Email { get; set; } 
        public string Password { get; set; } = "Test@123"; 
        public int? ShiftId1 { get; set; }
        public int? ShiftId2 { get; set; } 
        public int? ShiftId3 { get; set; } 
        public int? LocationId { get; set; } 
        public int? OverTime { get; set; } 
        public decimal? HourlyRate { get; set; } 
        public int Type { get; set; } = 5; 
        public string? EmployeeId { get; set; } 
    }
}
