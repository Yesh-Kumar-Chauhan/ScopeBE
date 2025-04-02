using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly AppDbContext _context;

        public CertificateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CertificateType>> GetAllCertificateTypeAsync()
        {
            return await _context.CertificateType.ToListAsync();
        }

        public async Task<List<Certificate>> GetAllAsync()
        {
            return await _context.Certificates.ToListAsync();
        }

        public async Task<Certificate?> GetByIdAsync(long certificateId)
        {
            return await _context.Certificates.FindAsync(certificateId);
        }

        public async Task<Certificate> AddAsync(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            await _context.SaveChangesAsync();
            return certificate;
        }

        public async Task<Certificate?> UpdateAsync(Certificate certificate)
        {
            var existingCertificate = await _context.Certificates.FindAsync(certificate.CertificateID);
            if (existingCertificate == null)
                return null;

            _context.Entry(existingCertificate).CurrentValues.SetValues(certificate);
            await _context.SaveChangesAsync();
            return existingCertificate;
        }

        public async Task<bool> DeleteAsync(long certificateId)
        {
            var certificate = await _context.Certificates.FindAsync(certificateId);
            if (certificate == null)
                return false;

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CertificateDto>> GetCertificatesByPersonIdAsync(long personId)
        {
            var certificateDtos = new List<CertificateDto>();

            using (var connection = _context.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[sp_Certificate_Select]";
                    command.Parameters.Add(new SqlParameter("@PersonID", personId));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var certificateDto = new CertificateDto
                            {
                                CertificateID = reader.GetInt64(reader.GetOrdinal("CertificateID")),
                                CertificateTypeID = reader.IsDBNull(reader.GetOrdinal("CertificateTypeID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("CertificateTypeID")),
                                PersonID = reader.GetInt64(reader.GetOrdinal("PersonID")),
                                CertificatePermanent = reader.GetBoolean(reader.GetOrdinal("CertificatePermanent")),
                                CertificateProfessional = reader.GetBoolean(reader.GetOrdinal("CertificateProfessional")),
                                CertificateCQ = reader.GetBoolean(reader.GetOrdinal("CertificateCQ")),
                                Initial = reader.GetBoolean(reader.GetOrdinal("Initial")),
                                InitialExpiration = reader.IsDBNull(reader.GetOrdinal("InitialExpiration")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("InitialExpiration")),
                                Provisional = reader.GetBoolean(reader.GetOrdinal("Provisional")),
                                ProvisionalExpiration = reader.IsDBNull(reader.GetOrdinal("ProvisionalExpiration")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ProvisionalExpiration")),
                                // Assuming CertificateTypeName is a field in the result of the stored procedure
                                CertificateTypeName = reader.IsDBNull(reader.GetOrdinal("CertificateTypeName")) ? null : reader.GetString(reader.GetOrdinal("CertificateTypeName"))
                            };

                            certificateDtos.Add(certificateDto);
                        }
                    }
                }
            }

            return certificateDtos;
        }
    }
}
