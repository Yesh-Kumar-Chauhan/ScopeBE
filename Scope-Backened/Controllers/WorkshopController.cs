using Core.DTOs.Workshop;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {
        private readonly IWorkshopService _workshopService;

        public WorkshopController(IWorkshopService workshopService)
        {
            _workshopService = workshopService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<WorkshopDto>>>> GetWorkshops(string? search = null, int? page = null, int? pageSize = null)
        {
            if (!page.HasValue || !pageSize.HasValue)
            {
                var response = await _workshopService.GetAllWorkshopsAsync(search);
                return Ok(response);
            }
            else
            {
                var response = await _workshopService.GetWorkshopsWithPaginationAsync(search, page.Value, pageSize.Value);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<WorkshopDto>>> GetWorkshopById(long id)
        {
            var response = await _workshopService.GetWorkshopByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<WorkshopFormDto>>> CreateWorkshop([FromBody] WorkshopFormDto workshopDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<WorkshopFormDto>(false, "Invalid data.", null));
            }

            var response = await _workshopService.CreateWorkshopAsync(workshopDto);
            return CreatedAtAction(nameof(GetWorkshopById), new { id = response.Data.WorkshopID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<WorkshopDto>>> UpdateWorkshop(long id, [FromBody] WorkshopFormDto workshopDto)
        {
            if (id != workshopDto.WorkshopID)
            {
                return BadRequest(new GenericResponse<WorkshopDto>(false, "Workshop ID mismatch.", null));
            }

            var response = await _workshopService.UpdateWorkshopAsync(id, workshopDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteWorkshop(long id)
        {
            var response = await _workshopService.DeleteWorkshopAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}
