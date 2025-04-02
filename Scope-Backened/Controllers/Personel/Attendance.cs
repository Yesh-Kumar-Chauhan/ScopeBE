using Core.DTOs.Personel;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Personel
{
    public partial class PersonelController : ControllerBase
    {
        [HttpGet("person-attendance/{personId}")]
        public async Task<IActionResult> GetAttendancesByPersonId(long personId)
        {
            var result = await _attendanceService.GetAttendancesByPersonIdAsync(personId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("attendance/{id}")]
        public async Task<IActionResult> GetAttendanceById(long id)
        {
            var result = await _attendanceService.GetAttendanceByIdAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("attendance")]
        public async Task<IActionResult> SaveAttendance([FromBody] AttendanceFormDto attendanceDto)
        {
            if (attendanceDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _attendanceService.SaveAttendanceAsync(attendanceDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("attendance/{id}")]
        public async Task<IActionResult> PutAttendance(long id, [FromBody] AttendanceFormDto attendanceDto)
        {
            if (attendanceDto == null || id != attendanceDto.AttendanceID)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _attendanceService.UpdateAttendanceAsync(attendanceDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("attendance/{id}")]
        public async Task<IActionResult> DeleteAttendance(long id)
        {
            var result = await _attendanceService.DeleteAttendanceAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("absences")]
        public async Task<IActionResult> GetAbsences()
        {
            var result = await _attendanceService.GetAbsencesAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("absence-reasons")]
        public async Task<IActionResult> GetAbsenceReasons()
        {
            var result = await _attendanceService.GetAbsenceReasonsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
