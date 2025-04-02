using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modals
{
    public class ExpungeLetter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MI { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime? DOB { get; set; }
        public string DistrictNameB { get; set; }
        public string DistrictNameD { get; set; }
        public string DistrictNameA { get; set; }
        public string SiteNameB { get; set; }
        public string SiteNameD { get; set; }
        public string SiteNameA { get; set; }
        public string SitePermitB { get; set; }
        public string SitePermitD { get; set; }
        public string SitePermitA { get; set; }
        public string SiteAddressB { get; set; }
        public string SiteAddressD { get; set; }
        public string SiteAddressA { get; set; }
        public string SiteBeforeClosed { get; set; }
        public string SiteDuringClosed { get; set; }
        public string SiteAfterClosed { get; set; }
    }
}
