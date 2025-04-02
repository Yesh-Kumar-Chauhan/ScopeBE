using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class School
    {
        [Key]
        public long SchoolID { get; set; }
        public string? SCH_NUM { get; set; }
        public string? SCH_NAM { get; set; }
        public string? PRINCIPAL { get; set; }
        public string? ADDR1 { get; set; }
        public string? ADDR2 { get; set; }
        public string? DIST_NUM { get; set; }
        public string? DIST_NAM { get; set; }
        public string? SITE_NUM { get; set; }
        public string? SITE_NAM { get; set; }
        public string? DISMISAL { get; set; }
        public string? TRANS { get; set; }
        public string? EMAIL { get; set; }
        public bool Hidden { get; set; }
        public string? Notes { get; set; }

    }
}
