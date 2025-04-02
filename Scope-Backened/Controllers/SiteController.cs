using Application.Services;
using Azure;
using Core.DTOs.Core.DTOs;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {
        private readonly ISiteService _siteService;

        public SiteController(ISiteService siteService)
        {
            _siteService = siteService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<SiteDto>>>> GetSites(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                // Retrieve all personnel
                var response = await _siteService.GetAllSitesAsync();
                return Ok(response);
            }
            else
            {
                // Retrieve filtered personnel with pagination
                var response = await _siteService.GetSitesWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<SiteDto>>> GetSiteById(long id)
        {
            var response = await _siteService.GetSiteByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("siteId")]
        public async Task<IActionResult> GetSiteByIds([FromBody] List<long?> ids)
        {
            var response = await _siteService.GetSitesByIdsAsync(ids);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<SiteDto>>> CreateSite([FromBody] SiteDto siteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<SiteDto>(false, "Invalid data.", null));
            }

            var response = await _siteService.CreateSiteAsync(siteDto);
            return CreatedAtAction(nameof(GetSiteById), new { id = response.Data.SiteID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<SiteDto>>> UpdateSite(long id, [FromBody] SiteDto siteDto)
        {
            if (id != siteDto.SiteID)
            {
                return BadRequest(new GenericResponse<SiteDto>(false, "Site ID mismatch.", null));
            }

            var response = await _siteService.UpdateSiteAsync(id, siteDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteSite(long id)
        {
            var response = await _siteService.DeleteSiteAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("district")]
        public async Task<ActionResult<GenericResponse<DataTable>>> GetSitesByDistrictId([FromQuery] long districtId, [FromQuery] long? districtNum = null, [FromQuery] int operation = 0)
        {
            var response = await _siteService.GetSitesByDistrictIdAsync(districtId, districtNum, operation);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("operation")]
        public async Task<ActionResult<GenericResponse<DataTable>>> GetSitesByOperationAsync([FromQuery] string? keyword, [FromQuery] int operation = 0)
        {
            var response = await _siteService.GetSitesByOperationAsync(keyword, operation);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
