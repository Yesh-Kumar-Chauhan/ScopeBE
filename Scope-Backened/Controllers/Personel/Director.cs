using Core.DTOs.Personel;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Personel
{
    public partial class PersonelController : ControllerBase
    {
        [HttpGet("person-directors/{personId}")]
        public async Task<IActionResult> GetDirectorsByPersonId(long personId)
        {
            var result = await _directorService.GetDirectorsByPersonIdAsync(personId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("director/{id}")]
        public async Task<IActionResult> GetDirectorById(long id)
        {
            var result = await _directorService.GetDirectorByIdAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("director")]
        public async Task<IActionResult> SaveDirector([FromBody] DirectorDto directorDto)
        {
            if (directorDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _directorService.SaveDirectorAsync(directorDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("director/{id}")]
        public async Task<IActionResult> UpdateDirector(long id, [FromBody] DirectorDto directorDto)
        {
            if (directorDto == null || id != directorDto.DirectorID)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _directorService.UpdateDirectorAsync(directorDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("director/{id}")]
        public async Task<IActionResult> DeleteDirector(long id)
        {
            var result = await _directorService.DeleteDirectorAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
