using Application.Services;
using Core.DTOs.Personel;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scope_Backened.Controllers.Personel
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class PersonelController : ControllerBase
    {
        private readonly IPersonelService _personelService;
        private readonly ITimesheetService _timesheetService;
        private readonly ICertificateService _certificateService;
        private readonly IAttendanceService _attendanceService;
        private readonly IWaiversSentService _waiversSentService;
        private readonly IDirectorService _directorService;

        public PersonelController(IPersonelService personelService,
            ITimesheetService timesheetService,
            ICertificateService certificateService,
            IAttendanceService attendanceService,
            IWaiversSentService waiversSentService,
            IDirectorService directorService
            )
        {
            _personelService = personelService;
            _timesheetService = timesheetService;
            _certificateService = certificateService;
            _attendanceService = attendanceService;
            _waiversSentService = waiversSentService;
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<PersonelDto>>>> GetPersonel(string? search = null, int? page = null, int? pageSize = null)
        {
            if (string.IsNullOrWhiteSpace(search) && !page.HasValue && !pageSize.HasValue)
            {
                // Retrieve all personnel
                var response = await _personelService.GetAllPersonelAsync();
                return Ok(response);
            }
            else
            {
                // Retrieve filtered personnel with pagination
                var response = await _personelService.GetFilteredPersonelAsync(search, page ?? 1, pageSize ?? 10);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<PersonelDto>>> GetPersonelById(long id)
        {
            var response = await _personelService.GetPersonelByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("ByEmail")]
        public async Task<ActionResult<GenericResponse<PersonelDto>>> GetPersonelByEmailAsync([FromQuery] string email)
        {
            var response = await _personelService.GetPersonelByEmailAsync(email);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpGet("personal-schedule")]
        public async Task<IActionResult> GetPersonelScheduleExcel()
        {
            var response = await _personelService.GetPersonelScheduleExcel();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<PersonelDto>>> CreatePersonel([FromBody] PersonelDto personelDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<PersonelDto>(false, "Invalid data.", null));
            }

            var response = await _personelService.CreatePersonelAsync(personelDto);
            if (response.Success)
            {
               return CreatedAtAction(nameof(GetPersonelById), new { id = response.Data.PersonalID }, response);
            }

            return BadRequest(new GenericResponse<PersonelDto>(false, response.Message ?? "An error occurred while creating the personnel.", null));

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<PersonelDto>>> UpdatePersonel(long id, [FromBody] PersonelDto personelDto)
        {
            if (id != personelDto.PersonalID)
            {
                return BadRequest(new GenericResponse<PersonelDto>(false, "Personel ID mismatch.", null));
            }

            var response = await _personelService.UpdatePersonelAsync(id, personelDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeletePersonel(long id)
        {
            var response = await _personelService.DeletePersonelAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
