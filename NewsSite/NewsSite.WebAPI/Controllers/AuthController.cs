using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginUserResponse), 200)]
        public async Task<ActionResult<LoginUserResponse>> LoginAsync([FromBody] UserLoginRequest userLoginRequest)
        {
            var response = await _authService.LoginAsync(userLoginRequest);

            return Ok(response);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(NewUserResponse), 201)]
        public async Task<ActionResult<NewUserResponse>> RegisterAsync([FromBody] UserRegisterRequest userRegisterRequest)
        {
            var response = await _authService.RegisterAsync(userRegisterRequest);

            return Created("register", response);
        }
    }
}
