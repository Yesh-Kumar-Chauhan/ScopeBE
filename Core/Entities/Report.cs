﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Report
    {
        [Key]
        public long ID { get; set; }
        public string? Name { get; set; }
    }
}
