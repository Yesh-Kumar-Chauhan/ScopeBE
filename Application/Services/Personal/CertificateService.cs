using AutoMapper;
using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Personal
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CertificateService> _logger;

        public CertificateService(ICertificateRepository certificateRepository, IMapper mapper, ILogger<CertificateService> logger)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<List<CertificateDto>>> GetCertificatesByPersonIdAsync(long personId)
        {
            try
            {
                var certificates = await _certificateRepository.GetCertificatesByPersonIdAsync(personId);
                return new GenericResponse<List<CertificateDto>>(true, "Certificates retrieved successfully.", certificates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving certificates.");
                return new GenericResponse<List<CertificateDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<CertificateType>>> GetAllCertificateTypesAsync()
        {
            try
            {
                var certificateTypes = await _certificateRepository.GetAllCertificateTypeAsync();
                return new GenericResponse<List<CertificateType>>(true, "Certificate types retrieved successfully.", certificateTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving certificate types.");
                return new GenericResponse<List<CertificateType>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<CertificateDto>> GetCertificateByIdAsync(long certificateId)
        {
            try
            {
                var certificate = await _certificateRepository.GetByIdAsync(certificateId);
                if (certificate == null)
                {
                    return new GenericResponse<CertificateDto>(false, "Certificate not found.", null);
                }

                var certificateDto = _mapper.Map<CertificateDto>(certificate);
                return new GenericResponse<CertificateDto>(true, "Certificate retrieved successfully.", certificateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the certificate.");
                return new GenericResponse<CertificateDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<CertificateFormDto>> SaveCertificateAsync(CertificateFormDto certificateDto)
        {
            try
            {
                var certificate = _mapper.Map<Certificate>(certificateDto);

                var newCertificate = await _certificateRepository.AddAsync(certificate);
                certificateDto = _mapper.Map<CertificateFormDto>(newCertificate);

                return new GenericResponse<CertificateFormDto>(true, "Certificate saved successfully.", certificateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the certificate.");
                return new GenericResponse<CertificateFormDto>(false, ex.Message, null);
            }
        }
        public async Task<GenericResponse<CertificateFormDto>> UpdateCertificateAsync(CertificateFormDto certificateDto)
        {
            try
            {
                var certificate = _mapper.Map<Certificate>(certificateDto);

                if (certificateDto.CertificateID > 0)
                {
                    var updatedCertificate = await _certificateRepository.UpdateAsync(certificate);
                    if (updatedCertificate == null)
                    {
                        return new GenericResponse<CertificateFormDto>(false, "Certificate not found.", null);
                    }
                    certificateDto = _mapper.Map<CertificateFormDto>(updatedCertificate);
                }

                return new GenericResponse<CertificateFormDto>(true, "Certificate saved successfully.", certificateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the certificate.");
                return new GenericResponse<CertificateFormDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<string>> DeleteCertificateAsync(long certificateId)
        {
            try
            {
                var success = await _certificateRepository.DeleteAsync(certificateId);
                if (!success)
                {
                    return new GenericResponse<string>(false, "Certificate not found.", null);
                }

                return new GenericResponse<string>(true, "Certificate deleted successfully.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the certificate.");
                return new GenericResponse<string>(false, ex.Message, null);
            }
        }
    }
}
