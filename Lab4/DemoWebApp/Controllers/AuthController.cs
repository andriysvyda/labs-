using Microsoft.AspNetCore.Mvc;
using DemoWebApp.Services;
using DemoWebApp.Models.Requests;

namespace DemoWebApp.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService service;

        public AuthController(IAuthService service)
        {
            this.service = service;
        }

        [Route("api/auth/login/{identityType}")]
        [HttpPost]
        public IActionResult Login(string identityType, [FromBody] LoginRequest body)
        {
            var user = service.Authenticate(identityType, body.Identity, body.Secret);

            if (user == null)
            {
                return Unauthorized(new Response()
                {
                    Status = 401
                });
            }

            return Ok(new Response()
            {
                Status = 200,
                Data = user
            });
        }
    }
}