using Core.DTOs;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IVisitService
    {
        Task<GenericResponse<IEnumerable<VisitDto>>> GetVisitsWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<VisitDto>>> GetAllVisitsAsync();
        Task<GenericResponse<VisitDto>> GetVisitByIdAsync(long id);
        Task<GenericResponse<VisitDto>> CreateVisitAsync(VisitDto visitDto);
        Task<GenericResponse<VisitDto>> UpdateVisitAsync(long id, VisitDto visitDto);
        Task<GenericResponse<bool>> DeleteVisitAsync(long id);
        Task<GenericResponse<IEnumerable<VisitDto>>> GetAllVisitsBySiteIdAsync(long siteId, string? search);
        Task<GenericResponse<IEnumerable<VisitDto>>> GetVisitsBySiteIdWithPaginationAsync(long siteId, string? search, int page, int pageSize);

    }
}
