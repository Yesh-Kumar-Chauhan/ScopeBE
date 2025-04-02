using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class WorkshopMember
    {
        [Key]
        public long WorkshopMemberID { get; set; }
        [ForeignKey("Workshop")]
        public long? WorkshopID { get; set; }
        public long? PersonID { get; set; }

        [JsonIgnore]
        public virtual Workshop Workshop { get; set; }
    }
}
