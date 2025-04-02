using Core.DTOs;
using Core.DTOs.Core.DTOs;
using Core.DTOs.Personel;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ITimesheetService
    {
        Task<GenericResponse<IEnumerable<TimesheetDto>>> GetTimesheetsWithFilterAsync(string? search, int page, int pageSize);
        Task<GenericResponse<List<TimesheetDto>>> GetAllTimesheetsAsync();
        Task<GenericResponse<TimesheetDto>> GetTimesheetByIdAsync(long id);
        Task<GenericResponse<TimesheetDto>> CreateTimesheetAsync(TimesheetDto timesheetDto);
        Task<GenericResponse<TimesheetDto>> UpdateTimesheetAsync(long id, TimesheetDto timesheetDto);
        Task<GenericResponse<bool>> DeleteTimesheetAsync(long id);
        Task<GenericResponse<IEnumerable<dynamic>>> GetTimesheetsByPersonIdAsync(long personId, DateTime? startDate, DateTime? endDate);
        Task<GenericResponse<List<ScheduleDto>>> GetScheduleTimesheetsByPersonId(long personId, DateTime date);
        Task<GenericResponse<bool>> UpdateSiteByTimesheetDateAsync();
        Task<GenericResponse<List<SiteTimesheetDto>>> GetSiteByTimesheetDate(DateTime date);
        Task<GenericResponse<TimesheetDto>> UpdateExternalTimesheet(long id, TimesheetDto timesheetDto);
        Task<GenericResponse<List<TimesheetDto>>> BulkUpdateTimesheetsAsync(List<TimesheetDto> timesheets);
        Task<GenericResponse<IEnumerable<dynamic>>> GetTimesheetsBySiteAndDistrictAsync(int siteId, int distNumber, DateTime? startDate, DateTime? endDate);
    }
}
