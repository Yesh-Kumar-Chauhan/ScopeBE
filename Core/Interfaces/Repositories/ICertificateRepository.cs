using Core.DTOs.Personel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ICertificateRepository
    {
        Task<List<CertificateType>> GetAllCertificateTypeAsync();
        Task<List<Certificate>> GetAllAsync();
        Task<Certificate?> GetByIdAsync(long certificateId);
        Task<Certificate> AddAsync(Certificate certificate);
        Task<Certificate?> UpdateAsync(Certificate certificate);
        Task<bool> DeleteAsync(long certificateId);
        Task<List<CertificateDto>> GetCertificatesByPersonIdAsync(long personId);
    }
}
