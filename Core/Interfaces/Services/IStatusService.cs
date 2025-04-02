using Core.DTOs;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IStatusService
    {
        Task<GenericResponse<IEnumerable<StatusDto>>> GetStatusesWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<StatusDto>>> GetAllStatusesAsync();
        Task<GenericResponse<StatusDto>> GetStatusByIdAsync(long id);
        Task<GenericResponse<StatusDto>> CreateStatusAsync(StatusDto statusDto);
        Task<GenericResponse<StatusDto>> UpdateStatusAsync(long id, StatusDto statusDto);
        Task<GenericResponse<bool>> DeleteStatusAsync(long id);
    }
}
