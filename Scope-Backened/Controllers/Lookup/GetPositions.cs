using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Lookup
{
    public partial class LookupController
    {
        [Route("GetPositions")]
        [HttpGet]
        public async Task<IActionResult> GetPositionsByType()
        {
            //var beforePositions = await _positionService.GetPositionsAsync("1");
            //var duringPositions = await _positionService.GetPositionsAsync("2");
            //var afterPositions = await _positionService.GetPositionsAsync("3");
            //return Ok(new { beforePositions  = beforePositions , duringPositions  = duringPositions, afterPositions = afterPositions });

            var types = new List<string> { "1", "2", "3" };
            var response = await _positionService.GetPositionsByTypesAsync(types);

            if (!response.Success)
            {
                return BadRequest(response);  // Return error response if not successful
            }

            return Ok(response);
        } 
        
        [HttpGet]
        public async Task<IActionResult> GetPositions()
        {

            var response = await _positionService.GetPositionsAsync();

            if (!response.Success)
            {
                return BadRequest(response);  // Return error response if not successful
            }

            return Ok(response);
        }
    }
}
