using Core.DTOs.Workshop;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IWorkshopService
    {
        Task<GenericResponse<List<WorkshopDto>>> GetWorkshopsWithPaginationAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<WorkshopDto>>> GetAllWorkshopsAsync(string? search);
        Task<GenericResponse<WorkshopDto>> GetWorkshopByIdAsync(long id);
        Task<GenericResponse<WorkshopFormDto>> CreateWorkshopAsync(WorkshopFormDto workshopDto);
        Task<GenericResponse<WorkshopDto>> UpdateWorkshopAsync(long id, WorkshopFormDto workshopDto);
        Task<GenericResponse<bool>> DeleteWorkshopAsync(long id);
    }

}
