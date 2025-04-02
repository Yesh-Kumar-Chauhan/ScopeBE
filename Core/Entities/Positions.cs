using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Positions
    {
        [Key]
        public int Id { get; set; }
        public int? PositionId { get; set; } 
        public string Position { get; set; } 
        public string Type { get; set; } 
    }
}
