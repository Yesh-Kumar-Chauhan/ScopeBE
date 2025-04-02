using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Inservice
{
    public class InserviceDto
    {
        public long InserviceID { get; set; }
        public long? StaffId { get; set; }
        public string? Training { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Hours { get; set; }
        public long? TopicId { get; set; }
        public string? Sponsor { get; set; }
        public long? WorkshopTypeId { get; set; }
        public string? Notes { get; set; }
        public string? Flag { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }

        // Fields for Personnel updates
        public DateTime? CPR { get; set; }
        public DateTime? SHarassmentExp { get; set; }
        public DateTime? SHarassmentExp2 { get; set; }
        public DateTime? FirstAid { get; set; }
        public DateTime? MatDate { get; set; }
        public DateTime? MatApp { get; set; }
        public DateTime? ACES { get; set; }
        public DateTime? Elaw { get; set; }
        public DateTime? Foundations { get; set; }
        public DateTime? Foundations15H { get; set; }
    }
}
