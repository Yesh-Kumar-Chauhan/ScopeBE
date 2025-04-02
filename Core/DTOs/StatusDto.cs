using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class StatusDto
    {
        public long StatusID { get; set; }
        public string? StatusName { get; set; }
        public bool Hidden { get; set; }
    }
}
