using Lab2.Services;
using Lab2.Models.Requests;
using Lab2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
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
