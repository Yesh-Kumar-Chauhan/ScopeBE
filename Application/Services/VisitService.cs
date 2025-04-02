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
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VisitService> _logger;

        public VisitService(IVisitRepository visitRepository, IMapper mapper, ILogger<VisitService> logger)
        {
            _visitRepository = visitRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<IEnumerable<VisitDto>>> GetVisitsWithFilterAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _visitRepository.GetAllVisits();

                // Implement filtering logic if required
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c => c.NAME.Contains(search));
                }
                var totalItems = await query.CountAsync();
                var visits = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                var visitDtos = _mapper.Map<IEnumerable<VisitDto>>(visits);

                return new GenericResponse<IEnumerable<VisitDto>>(true, "Visits retrieved successfully.", visitDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all visits.");
                return new GenericResponse<IEnumerable<VisitDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<VisitDto>>> GetAllVisitsAsync()
        {
            try
            {
                var visits = await _visitRepository.GetAllVisitsAsync();
                var visitDtos = _mapper.Map<List<VisitDto>>(visits);

                return new GenericResponse<List<VisitDto>>(true, "All visits retrieved successfully.", visitDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all visits.");
                return new GenericResponse<List<VisitDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<VisitDto>> GetVisitByIdAsync(long id)
        {
            try
            {
                var visit = await _visitRepository.GetVisitByIdAsync(id);
                if (visit == null)
                {
                    _logger.LogWarning("Visit with ID {VisitId} not found.", id);
                    return new GenericResponse<VisitDto>(false, "Visit not found.", null);
                }

                var visitDto = _mapper.Map<VisitDto>(visit);
                return new GenericResponse<VisitDto>(true, "Visit retrieved successfully.", visitDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving visit with ID {VisitId}.", id);
                return new GenericResponse<VisitDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<VisitDto>> CreateVisitAsync(VisitDto visitDto)
        {
            try
            {
                var visit = _mapper.Map<Visit>(visitDto);
                var createdVisit = await _visitRepository.AddVisitAsync(visit);
                var createdVisitDto = _mapper.Map<VisitDto>(createdVisit);

                return new GenericResponse<VisitDto>(true, "Visit created successfully.", createdVisitDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new visit.");
                return new GenericResponse<VisitDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<VisitDto>> UpdateVisitAsync(long id, VisitDto visitDto)
        {
            try
            {
                var visit = _mapper.Map<Visit>(visitDto);
                visit.VisitID = id;

                var updatedVisit = await _visitRepository.UpdateVisitAsync(visit);
                if (updatedVisit == null)
                {
                    _logger.LogWarning("Visit with ID {VisitId} not found or update failed.", id);
                    return new GenericResponse<VisitDto>(false, "Visit not found or update failed.", null);
                }

                var updatedVisitDto = _mapper.Map<VisitDto>(updatedVisit);
                return new GenericResponse<VisitDto>(true, "Visit updated successfully.", updatedVisitDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating visit with ID {VisitId}.", id);
                return new GenericResponse<VisitDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteVisitAsync(long id)
        {
            try
            {
                var deleted = await _visitRepository.DeleteVisitAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("Visit with ID {VisitId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "Visit not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "Visit deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting visit with ID {VisitId}.", id);
                return new GenericResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<GenericResponse<IEnumerable<VisitDto>>> GetAllVisitsBySiteIdAsync(long siteId, string? search)
        {
            try
            {
                var query = _visitRepository.GetVisitsBySiteId(siteId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(v => v.NAME.Contains(search) || v.NOTES.Contains(search));
                }

                var visits = await query.ToListAsync();
                var visitDtos = _mapper.Map<IEnumerable<VisitDto>>(visits);

                return new GenericResponse<IEnumerable<VisitDto>>(true, "Visits retrieved successfully.", visitDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all visits by Site ID.");
                return new GenericResponse<IEnumerable<VisitDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<IEnumerable<VisitDto>>> GetVisitsBySiteIdWithPaginationAsync(long siteId, string? search, int page, int pageSize)
        {
            try
            {
                var query = _visitRepository.GetVisitsBySiteId(siteId);

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(v => v.NAME.Contains(search) || v.NOTES.Contains(search));
                }

                var totalItems = await query.CountAsync();
                var visits = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                var visitDtos = _mapper.Map<IEnumerable<VisitDto>>(visits);

                return new GenericResponse<IEnumerable<VisitDto>>(true, "Visits retrieved successfully.", visitDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving visits by Site ID with pagination.");
                return new GenericResponse<IEnumerable<VisitDto>>(false, ex.Message, null);
            }
        }

    }
}
