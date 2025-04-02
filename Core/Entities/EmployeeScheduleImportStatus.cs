using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class EmployeeScheduleImportStatus
    {
        public Guid Id { get; set; }
        public long PersonId { get; set; }
        public string SiteType { get; set; }
        public DateTime ImportedOn { get; set; } = DateTime.Now;

        public string ImportStatus { get; set; }


    }
}
