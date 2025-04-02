using Core.DTOs;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<VisitDto>>>> GetVisits(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                var response = await _visitService.GetAllVisitsAsync();
                return Ok(response);
            }
            else
            {
                var response = await _visitService.GetVisitsWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<VisitDto>>> GetVisitById(long id)
        {
            var response = await _visitService.GetVisitByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<VisitDto>>> CreateVisit([FromBody] VisitDto visitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<VisitDto>(false, "Invalid data.", null));
            }

            var response = await _visitService.CreateVisitAsync(visitDto);
            return CreatedAtAction(nameof(GetVisitById), new { id = response.Data.VisitID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<VisitDto>>> UpdateVisit(long id, [FromBody] VisitDto visitDto)
        {
            if (id != visitDto.VisitID)
            {
                return BadRequest(new GenericResponse<VisitDto>(false, "Visit ID mismatch.", null));
            }

            var response = await _visitService.UpdateVisitAsync(id, visitDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteVisit(long id)
        {
            var response = await _visitService.DeleteVisitAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("site/{siteId}")]
        public async Task<ActionResult<GenericResponse<IEnumerable<VisitDto>>>> GetVisitsBySiteId(
           long siteId,
           string? search = null,
           int? page = null,
           int? pageSize = null)
        {
            if (!page.HasValue || !pageSize.HasValue)
            {
                // Get all visits if pagination parameters are not provided
                var response = await _visitService.GetAllVisitsBySiteIdAsync(siteId, search);
                return Ok(response);
            }
            else
            {
                // Apply pagination if both parameters are provided
                var response = await _visitService.GetVisitsBySiteIdWithPaginationAsync(siteId, search, page.Value, pageSize.Value);
                return Ok(response);
            }
        }

    }
}
