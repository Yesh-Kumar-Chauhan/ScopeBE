using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Inservice
    {
        [Key]
        public long InserviceID { get; set; }
        public long? STAFF_ID { get; set; }
        public string? TRAINING { get; set; }
        public DateTime? DATE { get; set; }
        public decimal? HOURS { get; set; }
        public long? TopicID { get; set; }
        public string? SPONSOR { get; set; }
        public long? WorkShopTypeID { get; set; }
        public string? NOTES { get; set; }
        public string? Flag { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
