using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modals
{
    public class LocationModal
    {
        [Key]
        public int id { get; set; }
        public int company_id { get; set; }
        public string location_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public int geofence_radius { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public long scope_site_id { get; set; }
    }
}
