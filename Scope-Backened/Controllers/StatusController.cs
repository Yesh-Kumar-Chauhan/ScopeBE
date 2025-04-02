using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTOs;
using Core.Interfaces.Services;
using Core.Modals;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<StatusDto>>>> GetStatuses(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                var response = await _statusService.GetAllStatusesAsync();
                return Ok(response);
            }
            else
            {
                var response = await _statusService.GetStatusesWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<StatusDto>>> GetStatusById(long id)
        {
            var response = await _statusService.GetStatusByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<StatusDto>>> CreateStatus([FromBody] StatusDto statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<StatusDto>(false, "Invalid data.", null));
            }

            var response = await _statusService.CreateStatusAsync(statusDto);
            return CreatedAtAction(nameof(GetStatusById), new { id = response.Data.StatusID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<StatusDto>>> UpdateStatus(long id, [FromBody] StatusDto statusDto)
        {
            if (id != statusDto.StatusID)
            {
                return BadRequest(new GenericResponse<StatusDto>(false, "Status ID mismatch.", null));
            }

            var response = await _statusService.UpdateStatusAsync(id, statusDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteStatus(long id)
        {
            var response = await _statusService.DeleteStatusAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
