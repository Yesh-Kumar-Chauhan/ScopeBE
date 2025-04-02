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
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StatusService> _logger;

        public StatusService(IStatusRepository statusRepository, IMapper mapper, ILogger<StatusService> logger)
        {
            _statusRepository = statusRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<IEnumerable<StatusDto>>> GetStatusesWithFilterAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _statusRepository.GetAllStatuses();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(s => s.StatusName.Contains(search));
                }

                var totalItems = await query.CountAsync();
                var statuses = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                var statusDtos = _mapper.Map<IEnumerable<StatusDto>>(statuses);

                return new GenericResponse<IEnumerable<StatusDto>>(true, "Statuses retrieved successfully.", statusDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all statuses.");
                return new GenericResponse<IEnumerable<StatusDto>>(false, "An error occurred while retrieving statuses.", null);
            }
        }

        public async Task<GenericResponse<List<StatusDto>>> GetAllStatusesAsync()
        {
            try
            {
                var statuses = await _statusRepository.GetAllStatusesAsync();
                var statusDtos = _mapper.Map<List<StatusDto>>(statuses);

                return new GenericResponse<List<StatusDto>>(true, "All statuses retrieved successfully.", statusDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all statuses.");
                return new GenericResponse<List<StatusDto>>(false, "An error occurred while retrieving statuses.", null);
            }
        }

        public async Task<GenericResponse<StatusDto>> GetStatusByIdAsync(long id)
        {
            try
            {
                var status = await _statusRepository.GetStatusByIdAsync(id);
                if (status == null)
                {
                    _logger.LogWarning("Status with ID {StatusId} not found.", id);
                    return new GenericResponse<StatusDto>(false, "Status not found.", null);
                }

                var statusDto = _mapper.Map<StatusDto>(status);
                return new GenericResponse<StatusDto>(true, "Status retrieved successfully.", statusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving status with ID {StatusId}.", id);
                return new GenericResponse<StatusDto>(false, "An error occurred while retrieving the status.", null);
            }
        }

        public async Task<GenericResponse<StatusDto>> CreateStatusAsync(StatusDto statusDto)
        {
            try
            {
                var status = _mapper.Map<Status>(statusDto);
                var createdStatus = await _statusRepository.AddStatusAsync(status);
                var createdStatusDto = _mapper.Map<StatusDto>(createdStatus);

                return new GenericResponse<StatusDto>(true, "Status created successfully.", createdStatusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new status.");
                return new GenericResponse<StatusDto>(false, "An error occurred while creating the status.", null);
            }
        }

        public async Task<GenericResponse<StatusDto>> UpdateStatusAsync(long id, StatusDto statusDto)
        {
            try
            {
                var status = _mapper.Map<Status>(statusDto);
                status.StatusID = id;

                var updatedStatus = await _statusRepository.UpdateStatusAsync(status);
                if (updatedStatus == null)
                {
                    _logger.LogWarning("Status with ID {StatusId} not found or update failed.", id);
                    return new GenericResponse<StatusDto>(false, "Status not found or update failed.", null);
                }

                var updatedStatusDto = _mapper.Map<StatusDto>(updatedStatus);
                return new GenericResponse<StatusDto>(true, "Status updated successfully.", updatedStatusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating status with ID {StatusId}.", id);
                return new GenericResponse<StatusDto>(false, "An error occurred while updating the status.", null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteStatusAsync(long id)
        {
            try
            {
                var deleted = await _statusRepository.DeleteStatusAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Status with ID {StatusId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Status not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Status deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting status with ID {StatusId}.", id);
                return new GenericResponse<bool>(false, "An error occurred while deleting the status.", false);
            }
        }
    }
}
