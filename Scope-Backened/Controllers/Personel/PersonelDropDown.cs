using Core.DTOs.Personel;
using Core.Modals;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Personel
{
    public partial class PersonelController
    {
        [HttpGet("search")]
        public async Task<ActionResult<GenericResponse<List<PersonelDto>>>> GetPersonelByKeywordAndOperation(
      [FromQuery] string? keyword,
      [FromQuery] int operation,
      [FromQuery] int? page = null,
      [FromQuery] int? pageSize = null)
        {
            var response = await _personelService.GetPersonelByKeywordAndOperationAsync(keyword, operation, page, pageSize);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
