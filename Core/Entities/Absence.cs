using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Absence
    {
        [Key]
        public long AbsentID { get; set; }
        public string? AbsentName { get; set; }
        public decimal? Weight { get; set; }
    }
}
