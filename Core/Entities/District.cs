using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class District
    {
        [Key]
        public long DistrictID { get; set; } 
        public long DIST_NUM { get; set; }
        public string? DIST_NAM { get; set; }
        public string? LIAISON { get; set; }
        public string? TITLE { get; set; }
        public string? SECRETARY { get; set; }
        public string? ADDR1 { get; set; }
        public string? ADDR2 { get; set; }
        public string? ADDR3 { get; set; }
        public string? LPHONE { get; set; }
        public string? LFAX { get; set; }
        public string? LIASON2 { get; set; }
        public string? TITLE2 { get; set; }
        public string? SECRETARY2 { get; set; }
        public string? ADDR12 { get; set; }
        public string? ADDR22 { get; set; }
        public string? ADDR32 { get; set; }
        public string? LPHONE2 { get; set; }
        public string? LPAX2 { get; set; }
        public string? SUPER { get; set; }
        public string? STREET { get; set; }
        public string? CITY { get; set; }
        public string? STATE { get; set; }
        public string? ZIP { get; set; }
        public string? PHONE { get; set; }
        public string? FAX { get; set; }
        public bool? CONTRACT { get; set; }
        public string? TERMS { get; set; }
        public string? KNDRGRTN { get; set; }
        public bool? ACTIVE { get; set; }
        public string? CLASS { get; set; }
        public string? COUNTY { get; set; }
        public DateTime? KINREG { get; set; }
        public string? KINPER { get; set; }
        public string? KINFON { get; set; }
        public string? RSPNSBL { get; set; }
        public string? EMAIL1 { get; set; }
        public string? EMAIL2 { get; set; }
        public string? NOTES { get; set; }
        public string? LEMAIL1 { get; set; }
        public string? LEMAIL2 { get; set; }
        public string? BHEMERCON { get; set; }
        public string? BHEMERFON { get; set; }
        public string? BUILDING { get; set; }
        public string? EMAILSUPER { get; set; }
        public string? SUPERVISOR { get; set; }
        public string? TRAINER { get; set; }
    }
}
