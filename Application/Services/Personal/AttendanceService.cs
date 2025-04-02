using AutoMapper;
using Core.DTOs.Personel;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Personal
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AttendanceService> _logger;

        public AttendanceService(
            IAttendanceRepository attendanceRepository,
            IMapper mapper,
            ILogger<AttendanceService> logger)
        {
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GenericResponse<List<AttendanceDto>>> GetAttendancesByPersonIdAsync(long personId)
        {
            try
            {
                var attendances = await _attendanceRepository.GetAttendancesByPersonIdAsync(personId);
                return new GenericResponse<List<AttendanceDto>>(true, "Attendances retrieved successfully.", attendances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving attendances.");
                return new GenericResponse<List<AttendanceDto>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<AttendanceDto>> GetAttendanceByIdAsync(long attendanceId)
        {
            try
            {
                var attendance = await _attendanceRepository.GetByIdAsync(attendanceId);
                if (attendance == null)
                {
                    return new GenericResponse<AttendanceDto>(false, "Attendance not found.", null);
                }

                var attendanceDto = _mapper.Map<AttendanceDto>(attendance);
                return new GenericResponse<AttendanceDto>(true, "Attendance retrieved successfully.", attendanceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the attendance.");
                return new GenericResponse<AttendanceDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<AttendanceFormDto>> SaveAttendanceAsync(AttendanceFormDto attendanceDto)
        {
            try
            {
                var attendance = _mapper.Map<Attendance>(attendanceDto);

                var newAttendance = await _attendanceRepository.AddAsync(attendance);
                attendanceDto = _mapper.Map<AttendanceFormDto>(newAttendance);

                return new GenericResponse<AttendanceFormDto>(true, "Attendance saved successfully.", attendanceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the attendance.");
                return new GenericResponse<AttendanceFormDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<AttendanceFormDto>> UpdateAttendanceAsync(AttendanceFormDto attendanceDto)
        {
            try
            {
                var attendance = _mapper.Map<Attendance>(attendanceDto);

                if (attendanceDto.AttendanceID > 0)
                {
                    var updatedAttendance = await _attendanceRepository.UpdateAsync(attendance);
                    if (updatedAttendance == null)
                    {
                        return new GenericResponse<AttendanceFormDto>(false, "Attendance not found.", null);
                    }
                    attendanceDto = _mapper.Map<AttendanceFormDto>(updatedAttendance);
                }

                return new GenericResponse<AttendanceFormDto>(true, "Attendance saved successfully.", attendanceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the attendance.");
                return new GenericResponse<AttendanceFormDto>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<string>> DeleteAttendanceAsync(long attendanceId)
        {
            try
            {
                var success = await _attendanceRepository.DeleteAsync(attendanceId);
                if (!success)
                {
                    return new GenericResponse<string>(false, "Attendance not found.", null);
                }

                return new GenericResponse<string>(true, "Attendance deleted successfully.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the attendance.");
                return new GenericResponse<string>(false, ex.Message, null);
            }
        }


        public async Task<GenericResponse<List<Absence>>> GetAbsencesAsync()
        {
            try
            {
                var absences = await _attendanceRepository.GetAbsencesAsync();
                return new GenericResponse<List<Absence>>(true, "Absences retrieved successfully.", absences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving absences.");
                return new GenericResponse<List<Absence>>(false, ex.Message, null);
            }
        }

        public async Task<GenericResponse<List<AbsenceReasons>>> GetAbsenceReasonsAsync()
        {
            try
            {
                var absenceReasons = await _attendanceRepository.GetAbsenceReasonsAsync();
                return new GenericResponse<List<AbsenceReasons>>(true, "Absence reasons retrieved successfully.", absenceReasons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving absence reasons.");
                return new GenericResponse<List<AbsenceReasons>>(false, ex.Message, null);
            }
        }
    }
}
