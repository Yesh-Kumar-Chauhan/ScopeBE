using Core.DTOs.Core.DTOs;
using Core.DTOs.Personel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SchoolDto
    {
        public long SchoolID { get; set; }
        public string? SchNum { get; set; }
        public string? SchNam { get; set; }
        public string? Principal { get; set; }
        public string? Addr1 { get; set; }
        public string? Addr2 { get; set; }
        public string? DistNum { get; set; }
        public string? DistNam { get; set; }
        public string? SiteNum { get; set; }
        public string? SiteNam { get; set; }
        public string? Dismisal { get; set; }
        public string? Trans { get; set; }
        public string? Email { get; set; }
        public bool Hidden { get; set; }
        public string? Notes { get; set; }
    }

}
