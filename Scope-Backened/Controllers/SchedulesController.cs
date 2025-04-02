using Core.DTOs;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedules()
        {
            return Ok(await _scheduleService.GetSchedulesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDto>> GetScheduleById(long id)
        {
            return Ok(await _scheduleService.GetScheduleByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule(ScheduleDto scheduleDto)
        {
           return Ok(await _scheduleService.AddScheduleAsync(scheduleDto));
        }

        [HttpPost("Additional")]
        public async Task<IActionResult> AddAdditionalSchedule(ScheduleDto scheduleDto)
        {
            return Ok(await _scheduleService.AddAdditionalSchedule(scheduleDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(long id, ScheduleDto scheduleDto)
        {
            if (id != scheduleDto.Id)
                return BadRequest();

            return Ok(await _scheduleService.UpdateScheduleAsync(scheduleDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(long id)
        {
            return Ok(await _scheduleService.DeleteScheduleAsync(id));
        }

        [HttpDelete("ClearSchedule/{personId}")]
        public async Task<IActionResult> ClearSchedule(long personId)
        {
            return Ok(await _scheduleService.ClearSchedule(personId));
        }

        [HttpGet("GetScheduleByPerson/{personId}")]
        public async Task<IActionResult> GetScheduleByPerson(long personId)
        {
            var schedule = await _scheduleService.GetScheduleByPersonIdAsync(personId);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpGet("GetScheduleByPerson/{personId}/{startDate}/{endDate}")]
        public async Task<IActionResult> GetScheduleByPersonAndDateRange(long personId, DateTime startDate, DateTime endDate)
        {
            var result = await _scheduleService.GetScheduleByPersonIdAndDateRangeAsync(personId, startDate, endDate);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportSchedules(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var response = await _scheduleService.ImportSchedulesAsync(file);

                if (response.Success)
                    return Ok(response);

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                // Log error (optional)
                return StatusCode(500, new { Message = "An internal server error occurred.", Error = ex.Message });
            }
        }


    }
}
