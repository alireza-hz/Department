using API_Dpepartment.Models.Auth;
using API_Dpepartment.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Dpepartment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            try
            {
                var res = _auth.Register(dto);
                return Ok(res);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var res = _auth.Login(dto);
            if (res == null) return Unauthorized();
            return Ok(res);
        }
    }
}
