using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceDto>> GetAttendancesByPersonIdAsync(long personId)
        {
            var attendanceDtos = new List<AttendanceDto>();

            using (var connection = _context.Database.GetDbConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[sp_Attendances_Select]";
                    command.Parameters.Add(new SqlParameter("@PersonID", personId));

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var attendanceDto = new AttendanceDto
                            {
                                AttendanceID = reader.IsDBNull(reader.GetOrdinal("AttendanceID")) ? 0 : reader.GetInt64(reader.GetOrdinal("AttendanceID")),
                                StaffId = reader.IsDBNull(reader.GetOrdinal("STAFF_ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("STAFF_ID")),
                                Date = reader.IsDBNull(reader.GetOrdinal("DATE")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DATE")),
                                Reason = reader.IsDBNull(reader.GetOrdinal("REASON")) ? null : reader.GetString(reader.GetOrdinal("REASON")),
                                Paid = reader.IsDBNull(reader.GetOrdinal("PAID")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("PAID")),
                                Charged = reader.IsDBNull(reader.GetOrdinal("CHARGED")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("CHARGED")),
                                Fraction = reader.IsDBNull(reader.GetOrdinal("FRACTION")) ? null : reader.GetString(reader.GetOrdinal("FRACTION")),
                                SiteNumber = reader.IsDBNull(reader.GetOrdinal("SITENUM")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("SITENUM")),
                                SiteName = reader.IsDBNull(reader.GetOrdinal("SITENAM")) ? null : reader.GetString(reader.GetOrdinal("SITENAM")),
                                ReasonID = reader.IsDBNull(reader.GetOrdinal("ReasonID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("ReasonID")),
                                AbsentID = reader.IsDBNull(reader.GetOrdinal("AbsentID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("AbsentID")),
                                AbsentName = reader.IsDBNull(reader.GetOrdinal("AbsentName")) ? null : reader.GetString(reader.GetOrdinal("AbsentName")),
                                Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Weight")),
                                AbsenceID = reader.IsDBNull(reader.GetOrdinal("ID")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("ID")),
                                AbsenceReason = reader.IsDBNull(reader.GetOrdinal("Reason")) ? null : reader.GetString(reader.GetOrdinal("Reason")),
                                ReasonName = reader.IsDBNull(reader.GetOrdinal("ReasonName")) ? null : reader.GetString(reader.GetOrdinal("ReasonName"))
                            };

                            attendanceDtos.Add(attendanceDto);
                        }
                    }
                }
            }

            return attendanceDtos;
        }

        public async Task<List<Attendance>> GetAllAsync()
        {
            return await _context.Attendance.ToListAsync();
        }

        public async Task<Attendance?> GetByIdAsync(long attendanceId)
        {
            return await _context.Attendance.FindAsync(attendanceId);
        }

        public async Task<Attendance> AddAsync(Attendance attendance)
        {
            _context.Attendance.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<Attendance?> UpdateAsync(Attendance attendance)
        {
            var existingAttendance = await _context.Attendance.FindAsync(attendance.AttendanceID);
            if (existingAttendance == null)
                return null;

            _context.Entry(existingAttendance).CurrentValues.SetValues(attendance);
            await _context.SaveChangesAsync();
            return existingAttendance;
        }

        public async Task<bool> DeleteAsync(long attendanceId)
        {
            var attendance = await _context.Attendance.FindAsync(attendanceId);
            if (attendance == null)
                return false;

            _context.Attendance.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Absence>> GetAbsencesAsync()
        {
            return await _context.Absences.ToListAsync();
        }

        public async Task<List<AbsenceReasons>> GetAbsenceReasonsAsync()
        {
            return await _context.AbsenceReasons.ToListAsync();
        }

        public async Task<List<Attendance>> GetAttendanceByPersonIdsAsync(List<long?> personIds)
        {
            return await _context.Attendance
               .Where(p => personIds.Contains(p.STAFF_ID)) // Filter by PersonIDs
               .ToListAsync();
        }
    }
}
