using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClosingService : IClosingService
    {
        private readonly IClosingRepository _closingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ClosingService> _logger;

        public ClosingService(IClosingRepository closingRepository, IMapper mapper, ILogger<ClosingService> logger)
        {
            _closingRepository = closingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<IEnumerable<ClosingDto>>> GetClosingsWithFilterAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _closingRepository.GetAllClosings();

                //if (!string.IsNullOrWhiteSpace(search))
                //{
                //    query = query.Where(c => c.ClosingName.Contains(search) || c.Description.Contains(search));
                //}

                var totalItems = await query.CountAsync();
                var closings = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                var closingDtos = _mapper.Map<IEnumerable<ClosingDto>>(closings);

                return new GenericResponse<IEnumerable<ClosingDto>>(true, "Closings retrieved successfully.", closingDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all closings.");
                return new GenericResponse<IEnumerable<ClosingDto>>(false, "An error occurred while retrieving closings.", null);
            }
        }

        public async Task<GenericResponse<List<ClosingDto>>> GetAllClosingsAsync()
        {
            try
            {
                var closings = await _closingRepository.GetAllClosingsAsync();
                var closingDtos = _mapper.Map<List<ClosingDto>>(closings);

                return new GenericResponse<List<ClosingDto>>(true, "All closings retrieved successfully.", closingDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all closings.");
                return new GenericResponse<List<ClosingDto>>(false, "An error occurred while retrieving closings.", null);
            }
        }

        public async Task<GenericResponse<ClosingDto>> GetClosingByIdAsync(long id)
        {
            try
            {
                var closing = await _closingRepository.GetClosingByIdAsync(id);
                if (closing == null)
                {
                    _logger.LogWarning("Closing with ID {ClosingId} not found.", id);
                    return new GenericResponse<ClosingDto>(false, "Closing not found.", null);
                }

                var closingDto = _mapper.Map<ClosingDto>(closing);
                return new GenericResponse<ClosingDto>(true, "Closing retrieved successfully.", closingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving closing with ID {ClosingId}.", id);
                return new GenericResponse<ClosingDto>(false, "An error occurred while retrieving the closing.", null);
            }
        }

        public async Task<GenericResponse<ClosingDto>> CreateClosingAsync(ClosingDto closingDto)
        {
            try
            {
                var closing = _mapper.Map<Closing>(closingDto);
                var createdClosing = await _closingRepository.AddClosingAsync(closing);
                var createdClosingDto = _mapper.Map<ClosingDto>(createdClosing);

                return new GenericResponse<ClosingDto>(true, "Closing created successfully.", createdClosingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new closing.");
                return new GenericResponse<ClosingDto>(false, "An error occurred while creating the closing.", null);
            }
        }

        public async Task<GenericResponse<ClosingDto>> UpdateClosingAsync(long id, ClosingDto closingDto)
        {
            try
            {
                var closing = _mapper.Map<Closing>(closingDto);
                closing.ClosingID = id;

                var updatedClosing = await _closingRepository.UpdateClosingAsync(closing);
                if (updatedClosing == null)
                {
                    _logger.LogWarning("Closing with ID {ClosingId} not found or update failed.", id);
                    return new GenericResponse<ClosingDto>(false, "Closing not found or update failed.", null);
                }

                var updatedClosingDto = _mapper.Map<ClosingDto>(updatedClosing);
                return new GenericResponse<ClosingDto>(true, "Closing updated successfully.", updatedClosingDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating closing with ID {ClosingId}.", id);
                return new GenericResponse<ClosingDto>(false, "An error occurred while updating the closing.", null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteClosingAsync(long id)
        {
            try
            {
                var deleted = await _closingRepository.DeleteClosingAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Closing with ID {ClosingId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Closing not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Closing deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting closing with ID {ClosingId}.", id);
                return new GenericResponse<bool>(false, "An error occurred while deleting the closing.", false);
            }
        }

        public async Task<GenericResponse<IEnumerable<ClosingDto>>> GetClosingsByDistrictIdAsync(long? districtId, int page, int pageSize)
        {
            try
            {
                var closings = await _closingRepository.GetClosingsByDistrictIdAsync(districtId);

                var totalItems = closings.Count;
                var paginatedClosings = closings.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                var closingDtos = _mapper.Map<IEnumerable<ClosingDto>>(paginatedClosings);

                return new GenericResponse<IEnumerable<ClosingDto>>(true, "Closings retrieved successfully.", closingDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving closings by district ID.");
                return new GenericResponse<IEnumerable<ClosingDto>>(false, "An error occurred while retrieving closings.", null);
            }
        }
    }
}
