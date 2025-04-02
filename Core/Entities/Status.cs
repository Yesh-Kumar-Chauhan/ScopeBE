using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Status
    {
        [Key]
        public long StatusID { get; set; }
        public string? StatusName { get; set; }
        public bool Hidden { get; set; }
    }
}
