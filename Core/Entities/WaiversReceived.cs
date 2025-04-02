using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class WaiversReceived
    {
        public long WaiversReceivedID { get; set; }
        public long? STAFF_ID { get; set; }
        public long? SIte_ID { get; set; }
        public DateTime? RECEIVED { get; set; }
    }
}
