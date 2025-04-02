using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Site
{
    public class SiteByType
    {
        public long SiteID { get; set; }
        public long SiteNumber { get; set; }
        public string SiteName { get; set; } = string.Empty;
    }
}
