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
    public class WorkshopTopic
    {
        [Key]
        public long WorkshopTopicID { get; set; }
        [ForeignKey("Workshop")]
        public long? WorkshopID { get; set; }
        public long? TopicID { get; set; }

        [JsonIgnore]
        public virtual Workshop Workshop { get; set; }
    }
}
