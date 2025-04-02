using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class WaiversSent
    {
        [Key]
        public long WaiversSentID { get; set; }
        public long? STAFF_ID { get; set; }
        public DateOnly? SENT { get; set; }
    }
}
