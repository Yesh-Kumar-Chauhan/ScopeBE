using Core.DTOs.Workshop;
using Core.Entities;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IWorkshopRepository
    {
        Task<GenericResponse<List<WorkshopDto>>> GetAllWorkshopsAsync(string? search);
        IQueryable<Workshop> GetAllWorkshops();
        Task<Workshop> GetWorkshopByIdAsync(long id);
        Task<Workshop> AddWorkshopAsync(Workshop workshop);
        Task<Workshop> UpdateWorkshopAsync(Workshop workshop);
        Task<bool> DeleteWorkshopAsync(long id);
        Task BulkInsertWorkshopsAsync(IEnumerable<Workshop> workshops);
        Task BulkInsertWorkshopTopicsAsync(IEnumerable<WorkshopTopic> workshops);
        Task BulkInsertWorkshopMembersAsync(IEnumerable<WorkshopMember> workshops);

        Task RemoveWorkshopTopicsAsync(IEnumerable<long> topicIds);
        Task RemoveWorkshopMembersAsync(IEnumerable<long> memberIds);
    }

}
