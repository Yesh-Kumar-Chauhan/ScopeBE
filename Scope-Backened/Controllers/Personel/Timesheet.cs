using Core.DTOs;
using Core.DTOs.Personel;
using Core.Modals;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Personel
{
    public partial class PersonelController
    {
        // Timesheet-related API methods

        [HttpGet("timesheets")]
        public async Task<ActionResult<GenericResponse<IEnumerable<TimesheetDto>>>> GetTimesheets(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                var response = await _timesheetService.GetAllTimesheetsAsync();
                return Ok(response);
            }
            else
            {
                var response = await _timesheetService.GetTimesheetsWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }

        [HttpGet("timesheets/{id}")]
        public async Task<ActionResult<GenericResponse<TimesheetDto>>> GetTimesheetById(long id)
        {
            var response = await _timesheetService.GetTimesheetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("timesheets")]
        public async Task<ActionResult<GenericResponse<TimesheetDto>>> CreateTimesheet([FromBody] TimesheetDto timesheetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<TimesheetDto>(false, "Invalid data.", null));
            }

            var response = await _timesheetService.CreateTimesheetAsync(timesheetDto);
            if (response.Success)
            {
                // Return CreatedAtAction if the response is successful
                return CreatedAtAction(nameof(GetTimesheetById), new { id = response.Data.TimesheetID }, response);
            }

            return Ok(response);
        }

        [HttpPut("timesheets/{id}")]
        public async Task<ActionResult<GenericResponse<TimesheetDto>>> UpdateTimesheet(long id, [FromBody] TimesheetDto timesheetDto)
        {
            if (id != timesheetDto.TimesheetID)
            {
                return BadRequest(new GenericResponse<TimesheetDto>(false, "Timesheet ID mismatch.", null));
            }

            var response = await _timesheetService.UpdateTimesheetAsync(id, timesheetDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        
        [HttpPut("timesheets-external-update/{id}")]
        public async Task<ActionResult<GenericResponse<TimesheetDto>>> UpdateExternalTimesheet(long id, [FromBody] TimesheetDto timesheetDto)
        {
            if (id != timesheetDto.ExternalEventId)
            {
                return BadRequest(new GenericResponse<TimesheetDto>(false, "Timesheet ID mismatch.", null));
            }

            var response = await _timesheetService.UpdateExternalTimesheet(id, timesheetDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("timesheets/bulk")]
        public async Task<ActionResult<GenericResponse<List<TimesheetDto>>>> BulkUpdateTimesheets([FromBody] List<TimesheetDto> timesheets)
        {
            if (timesheets == null || !timesheets.Any())
            {
                return BadRequest(new GenericResponse<List<TimesheetDto>>(false, "No timesheets provided for update.", null));
            }

            var response = await _timesheetService.BulkUpdateTimesheetsAsync(timesheets);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpDelete("timesheets/{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteTimesheet(long id)
        {
            var response = await _timesheetService.DeleteTimesheetAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("timesheets-personal")]
        public async Task<IActionResult> GetTimesheetsByPersonId(long personId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var response = await _timesheetService.GetTimesheetsByPersonIdAsync(personId, startDate, endDate);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("timesheets-site-personel")]
        public async Task<IActionResult> GetTimesheetsBySiteAndDistrictAsync(int siteId, int distNumber, DateTime? startDate = null, DateTime? endDate = null)
        {
            var response = await _timesheetService.GetTimesheetsBySiteAndDistrictAsync(siteId, distNumber, startDate, endDate);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("timesheets-personal-schedule")]
        public async Task<IActionResult> GetScheduleTimesheetsByPersonId(long personId, DateTime date)
        {
            var response = await _timesheetService.GetScheduleTimesheetsByPersonId(personId, date);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        
        [HttpGet("timesheets-site-update")]
        public async Task<IActionResult> UpdateSiteByTimesheetDate()
        {
            var response = await _timesheetService.UpdateSiteByTimesheetDateAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("timesheets-site-schedule")]
        public async Task<IActionResult> GetSiteByTimesheetDate(DateTime date)
        {
            var response = await _timesheetService.GetSiteByTimesheetDate(date);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
