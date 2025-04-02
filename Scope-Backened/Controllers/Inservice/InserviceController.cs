using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Core.Modals;
using Core.DTOs.Inservice;
using Core.Interfaces.Services;

namespace Scope_Backened.Controllers.Inservice
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class InserviceController : ControllerBase
    {
        private readonly IInserviceService _inserviceService;
        private readonly IInserviceDropdownService _inserviceDropdownService;

        public InserviceController(IInserviceService inserviceService, IInserviceDropdownService inserviceDropdownService)
        {
            _inserviceService = inserviceService;
            _inserviceDropdownService = inserviceDropdownService;
        }
        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<InserviceDto>>>> GetInservices(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                var response = await _inserviceService.GetAllInservicesAsync();
                return Ok(response);
            }
            else
            {
                var response = await _inserviceService.GetInservicesWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<InserviceDto>>> GetInserviceById(long id)
        {
            var response = await _inserviceService.GetInserviceByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<InserviceDto>>> CreateInservice([FromBody] InserviceDto inserviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<InserviceDto>(false, "Invalid data.", null));
            }

            var response = await _inserviceService.CreateInserviceAsync(inserviceDto);
            return CreatedAtAction(nameof(GetInserviceById), new { id = response.Data.InserviceID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<InserviceDto>>> UpdateInservice(long id, [FromBody] InserviceDto inserviceDto)
        {
            if (id != inserviceDto.InserviceID)
            {
                return BadRequest(new GenericResponse<InserviceDto>(false, "Inservice ID mismatch.", null));
            }

            var response = await _inserviceService.UpdateInserviceAsync(id, inserviceDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteInservice(long id)
        {
            var response = await _inserviceService.DeleteInserviceAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("inservice-select")]
        public async Task<ActionResult<GenericResponse<List<InserviceDto>>>> GetInserviceSelect([FromQuery] long personId, [FromQuery] int operation)
        {
            var response = await _inserviceService.GetInserviceSelectAsync(personId, operation);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("bulk-create-inservices")]
        public async Task<ActionResult<GenericResponse<bool>>> BulkCreateInservicesAndUpdatePersonnel([FromBody] InserviceBulkDto formDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<bool>(false, "Invalid data.", false));
            }

            var response = await _inserviceService.BulkCreateInservicesAndUpdatePersonnelAsync(formDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
