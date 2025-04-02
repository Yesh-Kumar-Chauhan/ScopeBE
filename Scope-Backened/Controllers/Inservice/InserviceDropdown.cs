using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Inservice
{
    public partial class InserviceController
    {
        
        [HttpGet("topics")]
        public async Task<ActionResult> GetTopicTypes()
        {
            var topics = await _inserviceDropdownService.GetTopicTypesAsync();
            return Ok(topics);
        }

        [HttpGet("workshops")]
        public async Task<ActionResult> GetWorkshopTypes()
        {
            var workshops = await _inserviceDropdownService.GetWorkshopTypesAsync();
            return Ok(workshops);
        }
    }
}
