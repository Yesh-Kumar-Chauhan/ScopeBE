using Core.DTOs.Personel;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task<List<AttendanceDto>> GetAttendancesByPersonIdAsync(long personId);
        Task<List<Attendance>> GetAllAsync();
        Task<Attendance?> GetByIdAsync(long attendanceId);
        Task<Attendance> AddAsync(Attendance attendance);
        Task<Attendance?> UpdateAsync(Attendance attendance);
        Task<bool> DeleteAsync(long attendanceId);

        Task<List<Absence>> GetAbsencesAsync();
        Task<List<AbsenceReasons>> GetAbsenceReasonsAsync();
        Task<List<Attendance>> GetAttendanceByPersonIdsAsync(List<long?> personIds);
    }
}
