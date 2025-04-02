using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Personel
{
    public class WaiversSentDto
    {
        public long WaiversSentID { get; set; }
        public long? StaffId { get; set; }
        public DateOnly? Sent { get; set; }
    }

}
