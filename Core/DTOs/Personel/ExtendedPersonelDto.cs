using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Personel
{
    public class ExtendedPersonelDto : PersonelDto
    {
        public string? FullName { get; set; }  
        public int? Total { get; set; }        
        public int? Active { get; set; }       
        public int? Terminated { get; set; }   
    }
}
