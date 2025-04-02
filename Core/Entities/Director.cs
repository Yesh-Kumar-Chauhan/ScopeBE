using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Director
    {
        [Key]
        public long DirectorID { get; set; }
        public long PersonID { get; set; }
        public long SiteID { get; set; }
        public string? MonAMFrom { get; set; }
        public string? MonAMTo { get; set; }
        public string? TueAMFrom { get; set; }
        public string? TueAMTo { get; set; }
        public string? WedAMFrom { get; set; }
        public string? WedAMTo { get; set; }
        public string? ThuAMFrom { get; set; }
        public string? ThuAMTo { get; set; }
        public string? FriAMFrom { get; set; }
        public string? FriAMTo { get; set; }
        public string? MonPMFrom { get; set; }
        public string? MonPMTo { get; set; }
        public string? TuePMFrom { get; set; }
        public string? TuePMTo { get; set; }
        public string? WedPMFrom { get; set; }
        public string? WedPMTo { get; set; }
        public string? ThuPMFrom { get; set; }
        public string? ThuPMTo { get; set; }
        public string? FriPMFrom { get; set; }
        public string? FriPMTo { get; set; }
    }
}
