using Core.DTOs;
using Core.Entities;
using Core.Modals;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<GenericResponse<IEnumerable<ScheduleDto>>> GetSchedulesAsync();
        Task<GenericResponse<ScheduleDto>> GetScheduleByIdAsync(long id);
        Task<GenericResponse<ScheduleDto>> AddScheduleAsync(ScheduleDto scheduleDto);
        Task<GenericResponse<ScheduleDto>> UpdateScheduleAsync(ScheduleDto scheduleDto);
        Task<GenericResponse<bool>> DeleteScheduleAsync(long id);
        Task<GenericResponse<List<ScheduleDto>>> GetScheduleByPersonIdAsync(long personId);
        Task<GenericResponse<List<ScheduleDto>>> GetScheduleByPersonIdAndDateRangeAsync(long personId, DateTime startDate, DateTime endDate);
        Task<GenericResponse<Dictionary<string, object>>> ImportSchedulesAsync(IFormFile file);
        Task<GenericResponse<bool>> ClearSchedule(long personnelId);
        Task<GenericResponse<List<ScheduleDto>>> AddAdditionalSchedule(ScheduleDto scheduleDto);
    }

}
