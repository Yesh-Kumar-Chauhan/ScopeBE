using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ISchedularTimesheetRepository
    {
        Task<IEnumerable<SchedularTimesheet>> GetSchedularTimesheetsAsync();
        Task<SchedularTimesheet> GetSchedularTimesheetByIdAsync(long id);
        Task<SchedularTimesheet> AddSchedularTimesheetAsync(SchedularTimesheet timesheet);
        Task<SchedularTimesheet> UpdateSchedularTimesheetAsync(SchedularTimesheet timesheet);
        Task<bool> DeleteSchedularTimesheetAsync(long id);
        Task<SchedularTimesheet> GetSchedularTimesheetsByScheduleIdAsync(long scheduleId);
        Task<List<SchedularTimesheet>> GetSchedularTimesheetsByPersonalIdAsync(long personId);
    }
}
