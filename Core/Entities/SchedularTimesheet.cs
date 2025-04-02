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
    public class SchedularTimesheet
    {
        [Key]
        public long Id { get; set; }    

        [ForeignKey("Schedule")]
        public long? ScheduleId { get; set; }
        [ForeignKey("Personel")]
        public long? PersonID { get; set; }

        [JsonIgnore]
        public virtual Personel Personel { get; set; }
        
        [JsonIgnore]
        public virtual Schedule Schedule { get; set; }

        [ForeignKey("Site")]
        public long? SiteID { get; set; }

        [JsonIgnore]
        public virtual Site Site { get; set; }

        public string? SiteType { get; set; }
        public string? Position { get; set; }


        public DateTime Date { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public TimeSpan? LunchIn { get; set; }
        public TimeSpan? LunchOut { get; set; }
        public TimeSpan? AdditionalStart { get; set; }
        public TimeSpan? AdditionalStop { get; set; }
    }
}
