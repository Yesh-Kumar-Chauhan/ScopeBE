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
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<SchoolDto>>>> GetSchoolsWithFilter(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                // Retrieve all personnel
                var response = await _schoolService.GetAllSchoolsAsync();
                return Ok(response);
            }
            else
            {
                // Retrieve filtered personnel with pagination
                var response = await _schoolService.GetSchoolsWithFilterAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
          
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<SchoolDto>>> GetSchoolById(long id)
        {
            var response = await _schoolService.GetSchoolByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<SchoolDto>>> CreateSchool([FromBody] SchoolDto schoolDto)
        {
            var response = await _schoolService.CreateSchoolAsync(schoolDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<SchoolDto>>> UpdateSchool(long id, [FromBody] SchoolDto schoolDto)
        {
            var response = await _schoolService.UpdateSchoolAsync(id, schoolDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteSchool(long id)
        {
            var response = await _schoolService.DeleteSchoolAsync(id);
            return Ok(response);
        }
    }
}
