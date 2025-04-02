using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Core.DTOs.Core.DTOs;
using DocumentFormat.OpenXml.Bibliography;

namespace Application.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DistrictService> _logger;

        public DistrictService(IDistrictRepository districtRepository, IMapper mapper, ILogger<DistrictService> logger)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<IEnumerable<DistrictDto>>> GetDistrictsWithFilterAsync(string? search, int page, int pageSize)
        {
            try
            {
                var query = _districtRepository.GetAllDistricts();

                // Apply filtering based on the search query
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(d => d.DIST_NUM.ToString().Contains(search) || d.DIST_NAM.Contains(search));
                }

                // Get the total count of items (before pagination)
                var totalItems = await query.CountAsync();

                // Apply pagination
                var districts = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


                var districtDtos = _mapper.Map<IEnumerable<DistrictDto>>(districts);
                return new GenericResponse<IEnumerable<DistrictDto>>(true, "Success", districtDtos, totalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all districts.");
                return new GenericResponse<IEnumerable<DistrictDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<DistrictDto>>> GetAllDistrictsAsync()
        {
            try
            {
                var districts = await _districtRepository.GetAllDistrictsAsync();
                var orderedDistrict = districts.OrderBy(r => r.DIST_NAM);
                var districtsDtos = _mapper.Map<List<DistrictDto>>(orderedDistrict);
                return new GenericResponse<List<DistrictDto>>(true, "All districts retrieved successfully.", districtsDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all districts.");
                return new GenericResponse<List<DistrictDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<DistrictDto>> GetDistrictByIdAsync(long id)
        {
            try
            {
                var district = await _districtRepository.GetDistrictByIdAsync(id);
                if (district == null)
                    return new GenericResponse<DistrictDto>(false, "District not found", null);

                var districtDto = _mapper.Map<DistrictDto>(district);
                return new GenericResponse<DistrictDto>(true, "Success", districtDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving district with ID {id}.");
                return new GenericResponse<DistrictDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<DistrictDto>> AddDistrictAsync(DistrictDto districtDto)
        {
            try
            {
                var district = _mapper.Map<District>(districtDto);
                district = await _districtRepository.AddDistrictAsync(district);
                var resultDto = _mapper.Map<DistrictDto>(district);
                return new GenericResponse<DistrictDto>(true, "District added successfully", resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new district.");
                return new GenericResponse<DistrictDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<DistrictDto>> UpdateDistrictAsync(DistrictDto districtDto)
        {
            try
            {
                var district = await _districtRepository.GetDistrictByIdAsync(districtDto.DistrictId.Value);
                if (district == null)
                    return new GenericResponse<DistrictDto>(false, "District not found", null);

                _mapper.Map(districtDto, district);
                district = await _districtRepository.UpdateDistrictAsync(district);
                var resultDto = _mapper.Map<DistrictDto>(district);
                return new GenericResponse<DistrictDto>(true, "District updated successfully", resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating district with ID {districtDto.DistrictId}.");
                return new GenericResponse<DistrictDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<bool>> DeleteDistrictAsync(long id)
        {
            try
            {
                var success = await _districtRepository.DeleteDistrictAsync(id);
                return new GenericResponse<bool>(success, success ? "District deleted successfully" : "District not found", success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting district with ID {id}.");
                return new GenericResponse<bool>(false, ex.Message, false);
            }
        }

        public async Task<GenericResponse<List<DistrictDto>>> GetDistrictsAsync(string keyword, int operation)
        {
            try
            {
                var districts = await _districtRepository.GetDistrictsAsync(keyword, operation);
                var districtDtos = _mapper.Map<List<DistrictDto>>(districts);

                return new GenericResponse<List<DistrictDto>>(true, "Districts retrieved successfully.", districtDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving districts.");
                return new GenericResponse<List<DistrictDto>>(false, "An error occurred while retrieving districts.", null);
            }
        }
    }
}
