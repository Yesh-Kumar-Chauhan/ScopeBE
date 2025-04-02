using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Workshop
{
    public class WorkshopDto
    {
        public long WorkshopID { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Hours { get; set; }
        public string? Sponsor { get; set; }
        public bool? Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? TypeID { get; set; }
        public string? WorkshopName { get; set; }
        public string? TopicNames { get; set; } // Concatenated topic names
        public int? MemberCount { get; set; } // Number of members
        public int? TopicCount { get; set; }
        public virtual ICollection<WorkshopMember> WorkshopMembers { get; set; }
        public virtual ICollection<WorkshopTopic> WorkshopTopics { get; set; }
    }
}
