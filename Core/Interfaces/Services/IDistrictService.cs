using Core.DTOs;
using Core.Modals;

namespace Core.Interfaces.Services
{
    public interface IDistrictService
    {
        Task<GenericResponse<IEnumerable<DistrictDto>>> GetDistrictsWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<DistrictDto>>> GetAllDistrictsAsync();
        Task<GenericResponse<DistrictDto>> GetDistrictByIdAsync(long id);
        Task<GenericResponse<DistrictDto>> AddDistrictAsync(DistrictDto districtDto);
        Task<GenericResponse<DistrictDto>> UpdateDistrictAsync(DistrictDto districtDto);
        Task<GenericResponse<bool>> DeleteDistrictAsync(long id);
        Task<GenericResponse<List<DistrictDto>>> GetDistrictsAsync(string keyword, int operation);
    }
}
