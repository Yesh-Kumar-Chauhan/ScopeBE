using Core.DTOs.Personel;
using Core.Entities;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ICertificateService
    {
        Task<GenericResponse<List<CertificateDto>>> GetCertificatesByPersonIdAsync(long personId);
        Task<GenericResponse<List<CertificateType>>> GetAllCertificateTypesAsync();
        Task<GenericResponse<CertificateDto>> GetCertificateByIdAsync(long certificateId);
        Task<GenericResponse<CertificateFormDto>> SaveCertificateAsync(CertificateFormDto certificateDto);
        Task<GenericResponse<CertificateFormDto>> UpdateCertificateAsync(CertificateFormDto certificateDto);
        Task<GenericResponse<string>> DeleteCertificateAsync(long certificateId);
    }
}
