using Application.Services;
using Core.Modals;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Scope_Backened.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenValidationService _tokenValidationService;

        public AuthController(TokenValidationService tokenValidationService)
        {
            _tokenValidationService = tokenValidationService;
        }

        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken([FromBody] TokenRequest request)
        {
            try
            {
                var result = await _tokenValidationService.ValidateTokenAsync(request.Token);
                if (result.Success)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                // Handle validation errors
                return Unauthorized(new GenericResponse<ValidatedUser>(false, $"Token validation failed: {ex.Message}", null));
            }
        }
    }
}
