using Core.DTOs.Personel;
using Core.Entities;
using Core.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IAttendanceService
    {
        Task<GenericResponse<List<AttendanceDto>>> GetAttendancesByPersonIdAsync(long personId);
        Task<GenericResponse<AttendanceDto>> GetAttendanceByIdAsync(long attendanceId);
        Task<GenericResponse<AttendanceFormDto>> SaveAttendanceAsync(AttendanceFormDto attendanceDto);
        Task<GenericResponse<AttendanceFormDto>> UpdateAttendanceAsync(AttendanceFormDto attendanceDto);
        Task<GenericResponse<string>> DeleteAttendanceAsync(long attendanceId);
        Task<GenericResponse<List<Absence>>> GetAbsencesAsync();
        Task<GenericResponse<List<AbsenceReasons>>> GetAbsenceReasonsAsync();
    }
}
