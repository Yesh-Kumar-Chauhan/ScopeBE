using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Workshop
    {
        [Key]
        public long WorkshopID { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Hours { get; set; }
        public string? Sponsor { get; set; }
        public bool? Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? TypeID { get; set; }
        public string? WorkshopName { get; set; }

        public virtual ICollection<WorkshopMember> WorkshopMembers { get; set; }
        public virtual ICollection<WorkshopTopic> WorkshopTopics { get; set; }
    }
}
