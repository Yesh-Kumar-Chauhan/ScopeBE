using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Brainyclock
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string? LocationName { get; set; }
        public string? Address { get; set; }
        public string? Pincode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public int? GeofenceRadius { get; set; }
        public long? SiteId { get; set; }
    }

}
