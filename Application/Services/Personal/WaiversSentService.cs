using AutoMapper;
using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Personal
{
    public class WaiversSentService : IWaiversSentService
    {
        private readonly IWaiversSentRepository _waiversSentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WaiversSentService> _logger;

        public WaiversSentService(IWaiversSentRepository waiversSentRepository, IMapper mapper, ILogger<WaiversSentService> logger)
        {
            _waiversSentRepository = waiversSentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<List<WaiversSentDto>>> GetWaiversByStaffIdAsync(long staffId)
        {
            try
            {
                var waivers = await _waiversSentRepository.GetWaiversByStaffIdAsync(staffId);
                var waiverDto = _mapper.Map<List<WaiversSentDto>>(waivers);
                return new GenericResponse<List<WaiversSentDto>>(true, "Waivers retrieved successfully.", waiverDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving waivers.");
                return new GenericResponse<List<WaiversSentDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<WaiversSentDto>> GetWaiverByIdAsync(long waiverId)
        {
            try
            {
                var waiver = await _waiversSentRepository.GetByIdAsync(waiverId);
                if (waiver == null)
                {
                    return new GenericResponse<WaiversSentDto>(false, "Waiver not found.", null);
                }

                var waiverDto = _mapper.Map<WaiversSentDto>(waiver);
                return new GenericResponse<WaiversSentDto>(true, "Waiver retrieved successfully.", waiverDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the waiver.");
                return new GenericResponse<WaiversSentDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<WaiversSentDto>> SaveWaiverAsync(WaiversSentDto waiverDto)
        {
            try
            {
                var waiver = _mapper.Map<WaiversSent>(waiverDto);

                var newWaiver = await _waiversSentRepository.AddAsync(waiver);
                waiverDto = _mapper.Map<WaiversSentDto>(newWaiver);

                return new GenericResponse<WaiversSentDto>(true, "Waiver saved successfully.", waiverDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the waiver.");
                return new GenericResponse<WaiversSentDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<WaiversSentDto>> UpdateWaiverAsync(WaiversSentDto waiverDto)
        {
            try
            {
                var waiver = _mapper.Map<WaiversSent>(waiverDto);

                if (waiverDto.WaiversSentID > 0)
                {
                    var updatedWaiver = await _waiversSentRepository.UpdateAsync(waiver);
                    if (updatedWaiver == null)
                    {
                        return new GenericResponse<WaiversSentDto>(false, "Waiver not found.", null);
                    }
                    waiverDto = _mapper.Map<WaiversSentDto>(updatedWaiver);
                }

                return new GenericResponse<WaiversSentDto>(true, "Waiver updated successfully.", waiverDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the waiver.");
                return new GenericResponse<WaiversSentDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<string>> DeleteWaiverAsync(long waiverId)
        {
            try
            {
                var success = await _waiversSentRepository.DeleteAsync(waiverId);
                if (!success)
                {
                    return new GenericResponse<string>(false, "Waiver not found.", null);
                }

                return new GenericResponse<string>(true, "Waiver deleted successfully.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the waiver.");
                return new GenericResponse<string>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<WaiversReceivedDto>>> GetWaiversReceivedByStaffId(long staffId)
        {
            try
            {
                var waivers = await _waiversSentRepository.GetWaiversReceivedByStaffId(staffId);
                var waiverDto = _mapper.Map<List<WaiversReceivedDto>>(waivers);
                return new GenericResponse<List<WaiversReceivedDto>>(true, "Waivers retrieved successfully.", waiverDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving waivers.");
                return new GenericResponse<List<WaiversReceivedDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<bool>> BulkUpdateWaiversReceivedAsync(List<WaiversReceivedDto> waiversDto)
        {
            try
            {
                var waivers = _mapper.Map<List<WaiversReceived>>(waiversDto);
                var result = await _waiversSentRepository.AddOrUpdateWaiversReceivedAsync(waivers);

                if (!result)
                {
                    return new GenericResponse<bool>(false, "Failed to update waivers.", false);
                }

                return new GenericResponse<bool>(true, "Waivers updated successfully.", result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating waivers.");
                return new GenericResponse<bool>(false, ex.Message, false);
            }
        }

    }
}
