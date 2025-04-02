using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IStatusRepository
    {
        IQueryable<Status> GetAllStatuses();
        Task<List<Status>> GetAllStatusesAsync();
        Task<Status?> GetStatusByIdAsync(long id);
        Task<Status> AddStatusAsync(Status status);
        Task<Status?> UpdateStatusAsync(Status status);
        Task<bool> DeleteStatusAsync(long id);
    }
}
