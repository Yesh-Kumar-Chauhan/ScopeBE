using Core.DTOs.Personel;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Personel
{
    public partial class PersonelController : ControllerBase
    {
        [HttpGet("staff-waivers/{staffId}")]
        public async Task<IActionResult> GetWaiversByStaffId(long staffId)
        {
            var result = await _waiversSentService.GetWaiversByStaffIdAsync(staffId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("staff-waivers-received/{staffId}")]
        public async Task<IActionResult> GetWaiversReceivedByStaffId(long staffId)
        {
            var result = await _waiversSentService.GetWaiversReceivedByStaffId(staffId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("waiver-received/bulk-update")]
        public async Task<IActionResult> BulkUpdateWaiversReceived([FromBody] List<WaiversReceivedDto> waiversDto)
        {
            if (waiversDto == null || !waiversDto.Any())
            {
                return BadRequest("Invalid or empty data.");
            }

            var result = await _waiversSentService.BulkUpdateWaiversReceivedAsync(waiversDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("waiver/{id}")]
        public async Task<IActionResult> GetWaiverById(long id)
        {
            var result = await _waiversSentService.GetWaiverByIdAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("waiver")]
        public async Task<IActionResult> SaveWaiver([FromBody] WaiversSentDto waiverDto)
        {
            if (waiverDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _waiversSentService.SaveWaiverAsync(waiverDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("waiver/{id}")]
        public async Task<IActionResult> UpdateWaiver(long id, [FromBody] WaiversSentDto waiverDto)
        {
            if (waiverDto == null || id != waiverDto.WaiversSentID)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _waiversSentService.UpdateWaiverAsync(waiverDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("waiver/{id}")]
        public async Task<IActionResult> DeleteWaiver(long id)
        {
            var result = await _waiversSentService.DeleteWaiverAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
