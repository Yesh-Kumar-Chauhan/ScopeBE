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
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<ContactDto>>>> GetContacts(string? search = null, int? page = null, int? pageSize = null)
        {
            if (!page.HasValue || !pageSize.HasValue)
            {
                var response = await _contactService.GetAllContactsAsync(search);
                return Ok(response);
            }
            else
            {
                var response = await _contactService.GetContactsWithPaginationAsync(search, page.Value, pageSize.Value);
                return Ok(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResponse<ContactDto>>> GetContactById(long id)
        {
            var response = await _contactService.GetContactByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<ContactDto>>> CreateContact([FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<ContactDto>(false, "Invalid data.", null));
            }

            var response = await _contactService.CreateContactAsync(contactDto);
            return CreatedAtAction(nameof(GetContactById), new { id = response.Data.ContactID }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResponse<ContactDto>>> UpdateContact(long id, [FromBody] ContactDto contactDto)
        {
            if (id != contactDto.ContactID)
            {
                return BadRequest(new GenericResponse<ContactDto>(false, "Contact ID mismatch.", null));
            }

            var response = await _contactService.UpdateContactAsync(id, contactDto);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteContact(long id)
        {
            var response = await _contactService.DeleteContactAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("site/{siteId}")]
        public async Task<ActionResult<GenericResponse<IEnumerable<ContactDto>>>> GetContactsBySiteId(
            long siteId,
            string? search = null,
            int? page = null,
            int? pageSize = null)
        {
            if (!page.HasValue || !pageSize.HasValue)
            {
                // Get all contacts if pagination parameters are not provided
                var response = await _contactService.GetAllContactsBySiteIdAsync(siteId, search);
                return Ok(response);
            }
            else
            {
                // Apply pagination if both parameters are provided
                var response = await _contactService.GetContactsBySiteIdWithPaginationAsync(siteId, search, page.Value, pageSize.Value);
                return Ok(response);
            }
        }
    }
}
