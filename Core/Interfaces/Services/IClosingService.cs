using Core.DTOs;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IClosingService
    {
        Task<GenericResponse<IEnumerable<ClosingDto>>> GetClosingsWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<ClosingDto>>> GetAllClosingsAsync();
        Task<GenericResponse<ClosingDto>> GetClosingByIdAsync(long id);
        Task<GenericResponse<ClosingDto>> CreateClosingAsync(ClosingDto closingDto);
        Task<GenericResponse<ClosingDto>> UpdateClosingAsync(long id, ClosingDto closingDto);
        Task<GenericResponse<bool>> DeleteClosingAsync(long id);
        Task<GenericResponse<IEnumerable<ClosingDto>>> GetClosingsByDistrictIdAsync(long? districtId, int page, int pageSize);
    }
}
