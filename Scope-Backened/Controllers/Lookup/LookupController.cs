using Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scope_Backened.Controllers.Lookup
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class LookupController : ControllerBase
    {
        private readonly IPositionService _positionService;
        private readonly ISiteService _siteService;

        public LookupController(IPositionService positionService, ISiteService siteService)
        {
            _positionService = positionService;
            _siteService = siteService;
        }
    }
}
