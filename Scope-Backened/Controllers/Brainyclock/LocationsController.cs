using Core.Entities.Brainyclock;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using RestSharp;
using Core.Modals;
using Core.Interfaces.Services.Brainyclock;

namespace Scope_Backened.Controllers.Brainyclock
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly MySqlDbContext _context;
        private readonly ILocationService _locationService;
        public LocationsController(MySqlDbContext context, ILocationService locationService)
        {
            _context = context;
            _locationService = locationService;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            return Ok(locations);
        }

        // GET: api/Locations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        [HttpGet("sync-site-location")]
        public async Task<ActionResult<Location>> SyncSiteLocation()
        {
            return Ok(await _locationService.SyncSiteAndLocationAsync());
        }


        [HttpPost("addlocation")]
        public async Task<IActionResult> AddLocation([FromBody] LocationModal locationInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _locationService.AddLocationAsync(locationInput));
        }
    }

   
}
