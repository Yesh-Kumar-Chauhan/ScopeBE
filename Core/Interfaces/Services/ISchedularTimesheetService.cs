using Core.DTOs;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ISchedularTimesheetService
    {
        Task<GenericResponse<IEnumerable<SchedularTimesheetDto>>> GetSchedularTimesheetsAsync();
        Task<GenericResponse<SchedularTimesheetDto>> GetSchedularTimesheetByIdAsync(long id);
        Task<GenericResponse<SchedularTimesheetDto>> AddSchedularTimesheetAsync(SchedularTimesheetDto timesheetDto);
        Task<GenericResponse<SchedularTimesheetDto>> UpdateSchedularTimesheetAsync(SchedularTimesheetDto timesheetDto);
        Task<GenericResponse<bool>> DeleteSchedularTimesheetAsync(long id);
        Task<GenericResponse<SchedularTimesheetDto>> GetSchedularTimesheetsByScheduleIdAsync(long scheduleId);
        Task<GenericResponse<List<SchedularTimesheetDto>>> GetSchedularTimesheetsByPersonalIdAsync(long personId);
    }

}
