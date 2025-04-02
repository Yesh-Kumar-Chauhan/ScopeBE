using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SchoolService> _logger;
        private readonly IConfiguration _configuration;

        public SchoolService(ISchoolRepository schoolRepository, IMapper mapper,
            ILogger<SchoolService> logger, IConfiguration configuration)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<GenericResponse<IEnumerable<SchoolDto>>> GetSchoolsWithFilterAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _schoolRepository.GetAllSchools();

                // Apply filtering based on the search query
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(s => s.SCH_NUM.ToString().Contains(search) || s.SCH_NAM.Contains(search));
                }

                // Get the total count of items (before pagination)
                var totalItems = await query.CountAsync();

                // Apply pagination
                var schools = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var schoolDtos = _mapper.Map<IEnumerable<SchoolDto>>(schools);
                return new GenericResponse<IEnumerable<SchoolDto>>(true, "Schools retrieved successfully.", schoolDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all schools.");
                return new GenericResponse<IEnumerable<SchoolDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<SchoolDto>>> GetAllSchoolsAsync()
        {
            try
            {
                var schools = await _schoolRepository.GetAllSchoolsAsync();

                var schoolDtos = _mapper.Map<List<SchoolDto>>(schools);
                return new GenericResponse<List<SchoolDto>>(true, "All schools retrieved successfully.", schoolDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all schools.");
                return new GenericResponse<List<SchoolDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<SchoolDto>> GetSchoolByIdAsync(long id)
        {
            try
            {
                var school = await _schoolRepository.GetSchoolByIdAsync(id);
                if (school == null)
                {
                    _logger.LogWarning("School with ID {SchoolId} not found.", id);
                    return new GenericResponse<SchoolDto>(false, "School not found.", null);
                }

                var schoolDto = _mapper.Map<SchoolDto>(school);
                return new GenericResponse<SchoolDto>(true, "School retrieved successfully.", schoolDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving school with ID {SchoolId}.", id);
                return new GenericResponse<SchoolDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<SchoolDto>> CreateSchoolAsync(SchoolDto schoolDto)
        {
            try
            {
                var school = _mapper.Map<School>(schoolDto);
                var createdSchool = await _schoolRepository.AddSchoolAsync(school);
                var createdSchoolDto = _mapper.Map<SchoolDto>(createdSchool);
                return new GenericResponse<SchoolDto>(true, "School created successfully.", createdSchoolDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new school.");
                return new GenericResponse<SchoolDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<SchoolDto>> UpdateSchoolAsync(long id, SchoolDto schoolDto)
        {
            try
            {
                var school = _mapper.Map<School>(schoolDto);
                school.SchoolID = id;

                var updatedSchool = await _schoolRepository.UpdateSchoolAsync(school);
                if (updatedSchool == null)
                {
                    _logger.LogWarning("School with ID {SchoolId} not found or update failed.", id);
                    return new GenericResponse<SchoolDto>(false, "School not found or update failed.", null);
                }

                var updatedSchoolDto = _mapper.Map<SchoolDto>(updatedSchool);
                return new GenericResponse<SchoolDto>(true, "School updated successfully.", updatedSchoolDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating school with ID {SchoolId}.", id);
                return new GenericResponse<SchoolDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteSchoolAsync(long id)
        {
            try
            {
                var deleted = await _schoolRepository.DeleteSchoolAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning("School with ID {SchoolId} not found or deletion failed.", id);
                    return new GenericResponse<bool>(false, "School not found or deletion failed.", false);
                }

                return new GenericResponse<bool>(true, "School deleted successfully.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting school with ID {SchoolId}.", id);
                return new GenericResponse<bool>(false, ex.Message, false);
            }
        }
    }
}
