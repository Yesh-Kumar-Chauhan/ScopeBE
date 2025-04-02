using Core.DTOs.Personel;
using Core.Interfaces.Services;
using Core.Modals;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Personel
{
    public partial class PersonelController : ControllerBase
    {
       

        [HttpGet("person-certificate/{personId}")]
        public async Task<IActionResult> GetCertificatesByPersonId(long personId)
        {
            var result = await _certificateService.GetCertificatesByPersonIdAsync(personId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("certificate-types")]
        public async Task<IActionResult> GetAllCertificateTypes()
        {
            var result = await _certificateService.GetAllCertificateTypesAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("certificate/{id}")]
        public async Task<IActionResult> GetCertificateById(long id)
        {
            var result = await _certificateService.GetCertificateByIdAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("certificate")]
        public async Task<IActionResult> SaveCertificate([FromBody] CertificateFormDto certificateDto)
        {
            if (certificateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _certificateService.SaveCertificateAsync(certificateDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("certificate/{id}")]
        public async Task<IActionResult> PutCertificate(long id, [FromBody] CertificateFormDto certificateDto)
        {
            if (certificateDto == null || id != certificateDto.CertificateID)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _certificateService.UpdateCertificateAsync(certificateDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("certificate/{id}")]
        public async Task<IActionResult> DeleteCertificate(long id)
        {
            var result = await _certificateService.DeleteCertificateAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
