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
    public class ClosingController : ControllerBase
    {
        private readonly IClosingService _closingService;

        public ClosingController(IClosingService closingService)
        {
            _closingService = closingService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<ClosingDto>>>> GetClosings(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                var response = await _closingService.GetAllClosingsAsync();
                return Ok(response);
            }
            else
            {
                var response = await _closingService.GetClosingsWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<ClosingDto>>> GetClosingById(long id)
        {
            var response = await _closingService.GetClosingByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{districtId}/closings")]
        public async Task<ActionResult<GenericResponse<IEnumerable<ClosingDto>>>> GetClosingsByDistrictId(long? districtId, int page = 1, int pageSize = 10)
        {
            var response = await _closingService.GetClosingsByDistrictIdAsync(districtId, page, pageSize);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<ClosingDto>>> CreateClosing([FromBody] ClosingDto closingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<ClosingDto>(false, "Invalid data.", null));
            }

            var response = await _closingService.CreateClosingAsync(closingDto);
            return CreatedAtAction(nameof(GetClosingById), new { id = response.Data.ClosingID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<ClosingDto>>> UpdateClosing(long id, [FromBody] ClosingDto closingDto)
        {
            if (id != closingDto.ClosingID)
            {
                return BadRequest(new GenericResponse<ClosingDto>(false, "Closing ID mismatch.", null));
            }

            var response = await _closingService.UpdateClosingAsync(id, closingDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteClosing(long id)
        {
            var response = await _closingService.DeleteClosingAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
