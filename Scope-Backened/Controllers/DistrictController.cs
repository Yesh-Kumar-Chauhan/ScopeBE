using Application.Services;
using Core.DTOs;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDistricts(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                // Retrieve all personnel
                var response = await _districtService.GetAllDistrictsAsync();
                return Ok(response);
            }
            else
            {
                // Retrieve filtered personnel with pagination
                var response = await _districtService.GetDistrictsWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDistrictById(long id)
        {
            var response = await _districtService.GetDistrictByIdAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddDistrict([FromBody] DistrictDto districtDto)
        {
            var response = await _districtService.AddDistrictAsync(districtDto);
            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetDistrictById), new { id = response.Data.DistrictId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDistrict(long id, [FromBody] DistrictDto districtDto)
        {
            if (id != districtDto.DistrictId)
                return BadRequest("District ID mismatch");

            var response = await _districtService.UpdateDistrictAsync(districtDto);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistrict(long id)
        {
            var response = await _districtService.DeleteDistrictAsync(id);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("operation")]
        public async Task<ActionResult<GenericResponse<List<DistrictDto>>>> GetDistricts(string? keyword = null, int operation = 0)
        {
            var response = await _districtService.GetDistrictsAsync(keyword, operation);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    }
}
