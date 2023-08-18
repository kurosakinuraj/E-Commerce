using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JwtAuthManager;
using JwtAuthManager.Models;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        public AccountController(JwtTokenHandler jwtAuthHandler)
        {
            _jwtTokenHandler = jwtAuthHandler;
        }

        [HttpPost]
        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest request)
        {
            var authenticationResponse = _jwtTokenHandler.GenerateToken(request);
            if (authenticationResponse == null) return Unauthorized();
            return Ok(authenticationResponse);
        }
    }
}
