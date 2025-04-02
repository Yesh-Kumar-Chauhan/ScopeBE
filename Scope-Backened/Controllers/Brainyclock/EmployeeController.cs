using Application.Services.Brainyclock;
using Core.Entities.Brainyclock;
using Core.Interfaces.Services.Brainyclock;
using Core.Modals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Brainyclock
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("sync-employees")]
        public async Task<ActionResult<GenericResponse<List<Employee>>>> SyncEmployees()
        {
            var response = await _employeeService.PostEmployeesToExternalApi();

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }
    }
}
