using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Report
{
    public class GenerateReportDto
    {
        public long ReportId { get; set; }
        public long? SiteId { get; set; }
        public long? PersonelId { get; set; }
        public long? PositionId { get; set; }
        public long? DistrictId { get; set; }
        public int? Type { get; set; }
        public string? Selections { get; set; }
        public DateTime? StartDate { get; set; }
        public string? CountryId { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
