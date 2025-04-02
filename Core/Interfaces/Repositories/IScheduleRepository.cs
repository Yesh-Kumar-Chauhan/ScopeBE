using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<IEnumerable<Schedule>> GetSchedulesAsync();
        Task<Schedule?> GetScheduleByIdAsync(long id);
        Task<Schedule?> AddScheduleAsync(Schedule schedule);
        Task<Schedule?> UpdateScheduleAsync(Schedule schedule);
        Task<bool> DeleteScheduleAsync(long id);
        Task<bool> DeleteScheduleBySessionIdAsync(Guid sessionId);
        Task<List<Schedule>> GetScheduleByPersonIdAsync(long personId);
        Task<List<Schedule>> GetScheduleByPersonIdAndDateRangeAsync(long personId, DateTime startDate, DateTime endDate);
        Task DeleteScheduleByEmployeeAsync(long personnelId);
        Task<List<Schedule>> GetSchedulesByPersonIdsAsync(List<long?> personIds);
        Task<List<Schedule>> AddSchedulesAsync(List<Schedule> schedulesToInsert);

    }
}
