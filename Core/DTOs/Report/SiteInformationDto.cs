using Core.DTOs.Core.DTOs;
using Core.DTOs.Personel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Report
{
    public class SiteInformationDto 
    {
        public DistrictDto District { get; set; }
        public SiteDto Site { get; set; }
        public PersonelDto Personnel { get; set; }

    }
}
