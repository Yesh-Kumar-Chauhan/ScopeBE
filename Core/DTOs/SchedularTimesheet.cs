using Core.DTOs.Core.DTOs;
using Core.DTOs.Personel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SchedularTimesheetDto
    {
        public long Id { get; set; }
        public long? ScheduleId { get; set; }
        public long? PersonID { get; set; }
        public long? SiteID { get; set; }
        public string? SiteType { get; set; }
        public string? Position { get; set; }


        public DateTime Date { get; set; }
        public TimeSpan? TimeIn { get; set; }
        public TimeSpan? TimeOut { get; set; }
        public TimeSpan? LunchIn { get; set; }
        public TimeSpan? LunchOut { get; set; }
        public TimeSpan? AdditionalStart { get; set; }
        public TimeSpan? AdditionalStop { get; set; }

        public PersonelDto? Personel { get; set; }
        public SiteDto? Site { get; set; }
        public ScheduleDto? Schedule { get; set; }

    }
}
