using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Workshop
{
    public class WorkshopFormDto
    {
        public long WorkshopID { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Hours { get; set; }
        public string? Sponsor { get; set; }
        public bool? Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? TypeID { get; set; }
        public string? WorkshopName { get; set; }
        public List<long> TopicIds { get; set; } = new List<long>();
        public List<long> PersonIds { get; set; } = new List<long>();
    }
}
