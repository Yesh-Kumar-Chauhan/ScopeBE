using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CertificateType
    {
        [Key]
        public long CertificateTypeID { get; set; }
        public string? CertificateTypeName { get; set; }
    }
}
