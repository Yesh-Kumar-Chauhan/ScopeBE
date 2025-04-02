using Core.DTOs;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedularTimesheetController : ControllerBase
    {
        private readonly ISchedularTimesheetService _schedularTimesheetService;

        public SchedularTimesheetController(ISchedularTimesheetService schedularTimesheetService)
        {
            _schedularTimesheetService = schedularTimesheetService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<SchedularTimesheetDto>>>> GetSchedularTimesheets()
        {
            var response = await _schedularTimesheetService.GetSchedularTimesheetsAsync();
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<SchedularTimesheetDto>>> GetSchedularTimesheetById(long id)
        {
            var response = await _schedularTimesheetService.GetSchedularTimesheetByIdAsync(id);
            if (!response.Success) return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<SchedularTimesheetDto>>> AddSchedularTimesheet(SchedularTimesheetDto timesheetDto)
        {
            var response = await _schedularTimesheetService.AddSchedularTimesheetAsync(timesheetDto);
            if (!response.Success) return BadRequest(response);

            return CreatedAtAction(nameof(GetSchedularTimesheetById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<SchedularTimesheetDto>>> UpdateSchedularTimesheet(long id, SchedularTimesheetDto timesheetDto)
        {
            if (id != timesheetDto.Id) return BadRequest("ID mismatch");

            var response = await _schedularTimesheetService.UpdateSchedularTimesheetAsync(timesheetDto);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteSchedularTimesheet(long id)
        {
            var response = await _schedularTimesheetService.DeleteSchedularTimesheetAsync(id);
            if (!response.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("BySchedule/{scheduleId}")]
        public async Task<IActionResult> GetSchedularTimesheetsByScheduleId(long scheduleId)
        {
            var response = await _schedularTimesheetService.GetSchedularTimesheetsByScheduleIdAsync(scheduleId);
            if (!response.Success) return NotFound(response);

            return Ok(response);
        }
        
        [HttpGet("ByPerson/{personId}")]
        public async Task<IActionResult> GetSchedularTimesheetsByPersonalIdAsync(long personId)
        {
            var response = await _schedularTimesheetService.GetSchedularTimesheetsByPersonalIdAsync(personId);
            if (!response.Success) return NotFound(response);

            return Ok(response);
        }

    }
}
