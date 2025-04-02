using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Personel
{
    public class WaiversReceivedDto
    {
        public long? WaiversReceivedID { get; set; }
        public long? StaffID { get; set; }
        public long? SiteID { get; set; }
        public DateTime? Received { get; set; }
        public string? DistName { get; set; }
        public string? SiteName { get; set; }
        public string? Country { get; set; }
    }
}
