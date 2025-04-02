using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Schedule
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Personel")]
        public long? PersonID { get; set; }

        [JsonIgnore]
        public virtual Personel Personel { get; set; }

        [ForeignKey("Site")]
        public long? SiteID { get; set; }

        [JsonIgnore]
        public virtual Site Site { get; set; }
        public string? SiteType { get; set; }
        public string? DeletedSiteType { get; set; }
        public string? Position { get; set; }
        public string? Notes { get; set; }

        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        //public TimeSpan? TimeIn { get; set; }
        //public TimeSpan? TimeOut { get; set; }
        //public TimeSpan? LunchIn { get; set; }
        //public TimeSpan? LunchOut { get; set; }
        //public TimeSpan? AdditionalStart { get; set; }
        //public TimeSpan? AdditionalStop { get; set; }

        public string? SiteName { get; set; }
        public long? DistNumber { get; set; }
        public string? DistName { get; set; }

        public TimeSpan? MondayTimeIn { get; set; }
        public TimeSpan? MondayTimeOut { get; set; }

        public TimeSpan? TuesdayTimeIn { get; set; }
        public TimeSpan? TuesdayTimeOut { get; set; }

        public TimeSpan? WednesdayTimeIn { get; set; }
        public TimeSpan? WednesdayTimeOut { get; set; }

        public TimeSpan? ThursdayTimeIn { get; set; }
        public TimeSpan? ThursdayTimeOut { get; set; }

        public TimeSpan? FridayTimeIn { get; set; }
        public TimeSpan? FridayTimeOut { get; set; }

        public string? Paycode { get; set; }
        public Guid? SessionId { get; set; }
    }
}
