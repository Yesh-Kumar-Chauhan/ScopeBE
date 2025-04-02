using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ITimesheetRepository
    {
        IQueryable<Timesheet> GetAllTimesheets();
        Task<List<Timesheet>> GetAllTimesheetsAsync();
        Task<Timesheet?> GetTimesheetByIdAsync(long id);
        Task<Timesheet?> GetTimesheetBySiteAndDateAsync(DateTime? date, long? siteId, long? personId);
        Task<Timesheet?> GetTimesheetByExternalIdAsync(long id);
        Task<Timesheet> AddTimesheetAsync(Timesheet timesheet);
        Task<Timesheet?> UpdateTimesheetAsync(Timesheet timesheet);
        Task<bool> DeleteTimesheetAsync(long id);
        Task<IEnumerable<dynamic>> GetTimesheetsByPersonIdAsync(long personId, DateTime? startDate, DateTime? endDate);
        Task<List<Timesheet>> GetTimesheetByDateAsync(DateTime date);
        Task<List<Timesheet>> BulkUpdateTimesheetsAsync(List<Timesheet> timesheets);
        Task<List<Timesheet>> BulkAddTimesheetsAsync(List<Timesheet> timesheetsToAdd);
        Task<IEnumerable<dynamic>> GetTimesheetsBySiteAndDistrictAsync(int siteId, int distNumber, DateTime? startDate, DateTime? endDate);
        
    }
}
