using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Personel
{
    public class CertificateDto
    {
        public long CertificateID { get; set; }
        public long? CertificateTypeID { get; set; }
        public long? PersonID { get; set; }
        public bool? CertificatePermanent { get; set; }
        public bool? CertificateProfessional { get; set; }
        public bool? CertificateCQ { get; set; }
        public bool? Initial { get; set; }
        public DateTime? InitialExpiration { get; set; }
        public bool? Provisional { get; set; }
        public DateTime? ProvisionalExpiration { get; set; }
        public string? CertificateTypeName { get; set; }

    }
    public class CertificateFormDto
    {
        public long CertificateID { get; set; }
        public long? CertificateTypeID { get; set; }
        public long? PersonID { get; set; }
        public bool? CertificatePermanent { get; set; }
        public bool? CertificateProfessional { get; set; }
        public bool? CertificateCQ { get; set; }
        public bool? Initial { get; set; }
        public DateTime? InitialExpiration { get; set; }
        public bool? Provisional { get; set; }
        public DateTime? ProvisionalExpiration { get; set; }

    }
}
