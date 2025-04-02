using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Brainyclock
{
    [Table("employeeSchedules")]
    public class EmployeeSchedules
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Employee_id")]
        public int EmployeeId { get; set; }

        [Column("Position")]
        [StringLength(255)]
        public string Position { get; set; }

        [Column("Notes")]
        public string Notes { get; set; }

        [Column("Date")]
        public DateTime? Date { get; set; }

        [Column("Site_id")]
        public int SiteId { get; set; }

        [Column("Site_type")]
        [StringLength(50)]
        public string SiteType { get; set; }

        [Column("End_date")]
        public DateTime? EndDate { get; set; }

        [Column("Start_date")]
        public DateTime? StartDate { get; set; }

        [Column("EventId")]
        public int? EventId { get; set; }

        [Column("siteName")]
        [StringLength(255)]
        public string SiteName { get; set; }

        [Column("distNumber")]
        [StringLength(50)]
        public string DistNumber { get; set; }

        [Column("distName")]
        [StringLength(255)]
        public string DistName { get; set; }

        [Column("MondayTimeIn")]
        public TimeSpan? MondayTimeIn { get; set; }

        [Column("MondayTimeOut")]
        public TimeSpan? MondayTimeOut { get; set; }

        [Column("TuesdayTimeIn")]
        public TimeSpan? TuesdayTimeIn { get; set; }

        [Column("TuesdayTimeOut")]
        public TimeSpan? TuesdayTimeOut { get; set; }

        [Column("WednesdayTimeIn")]
        public TimeSpan? WednesdayTimeIn { get; set; }

        [Column("WednesdayTimeOut")]
        public TimeSpan? WednesdayTimeOut { get; set; }

        [Column("ThursdayTimeIn")]
        public TimeSpan? ThursdayTimeIn { get; set; }

        [Column("ThursdayTimeOut")]
        public TimeSpan? ThursdayTimeOut { get; set; }

        [Column("FridayTimeIn")]
        public TimeSpan? FridayTimeIn { get; set; }

        [Column("FridayTimeOut")]
        public TimeSpan? FridayTimeOut { get; set; }

        [Column("DeletedSiteType")]
        [StringLength(50)]
        public string DeletedSiteType { get; set; }

        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Column("DeletedDate")]
        public DateTime? DeletedDate { get; set; }

        [Column("Paycode")]
        [StringLength(255)]
        public string Paycode { get; set; }
    }
}
