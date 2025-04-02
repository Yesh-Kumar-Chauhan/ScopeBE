using Core.DTOs.Report;
using Core.Interfaces.Services;
using Core.Modals;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IWebHostEnvironment _env;

        public ReportController(IReportService reportService, IWebHostEnvironment env)
        {
            _reportService = reportService;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<ReportDto>>>> GetAllReports()
        {
            var response = await _reportService.GetAllReportsAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<ReportDto>>> GetReportById(long id)
        {
            var response = await _reportService.GetReportByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<ReportDto>>> CreateReport([FromBody] ReportDto reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<ReportDto>(false, "Invalid data.", null));
            }

            var response = await _reportService.CreateReportAsync(reportDto);
            return CreatedAtAction(nameof(GetReportById), new { id = response.Data.ID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<ReportDto>>> UpdateReport(long id, [FromBody] ReportDto reportDto)
        {
            if (id != reportDto.ID)
            {
                return BadRequest(new GenericResponse<ReportDto>(false, "Report ID mismatch.", null));
            }

            var response = await _reportService.UpdateReportAsync(id, reportDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteReport(long id)
        {
            var response = await _reportService.DeleteReportAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("generate-report")]
        public async Task<IActionResult> GenerateReports([FromForm] GenerateReportDto reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<bool>(false, "Invalid data.", false));
            }

            var response = await _reportService.GenerateReportAsync(reportDto);

            return Ok(response);
        }

        [HttpGet("GetDirectories")]
        public IActionResult GetDirectories()
        {
            var wwwRootPath = _env.ContentRootPath;

            // Ensure the Templates directory exists
            var templatesPath = Path.Combine(wwwRootPath, "Templates");
            if (!Directory.Exists(templatesPath))
            {
                Directory.CreateDirectory(templatesPath);
            }

            // Return the list as JSON
            return Ok();
        }

    }
}
