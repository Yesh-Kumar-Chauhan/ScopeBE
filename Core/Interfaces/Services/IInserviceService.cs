using Core.DTOs.Inservice;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IInserviceService
    {
        Task<GenericResponse<IEnumerable<InserviceDto>>> GetInservicesWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<InserviceDto>>> GetAllInservicesAsync();
        Task<GenericResponse<InserviceDto>> GetInserviceByIdAsync(long id);
        Task<GenericResponse<InserviceDto>> CreateInserviceAsync(InserviceDto inserviceDto);
        Task<GenericResponse<InserviceDto>> UpdateInserviceAsync(long id, InserviceDto inserviceDto);
        Task<GenericResponse<bool>> DeleteInserviceAsync(long id);

        Task<GenericResponse<List<InserviceDto>>> GetInserviceSelectAsync(long personId, int operation);
        Task<GenericResponse<bool>> BulkCreateInservicesAndUpdatePersonnelAsync(InserviceBulkDto formDto);
    }
}
