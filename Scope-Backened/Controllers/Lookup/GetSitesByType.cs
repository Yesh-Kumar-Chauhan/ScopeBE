using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Lookup
{
    public partial class LookupController
    {
        [Route("GetSitesByType")]
        [HttpGet]
        public async Task<IActionResult> GetSitesByTypeAsync()
        {
            var types = new List<int> { 4, 5, 6 };
            var response = await _siteService.GetSitesByTypesAsync(types);

            if (!response.Success)
            {
                return BadRequest(new { message = response.Message });
            }

            return Ok(response);
        }
    }
}
