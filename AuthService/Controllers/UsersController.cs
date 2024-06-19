using AuthService.Domain.Requests.Users;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthService.Controllers
{
    //Авторизация и действия с пользователями
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : Controller
    {

        private IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _userService.Login(model);
            if (response.StatusCode == Domain.Enums.StatusCode.InternalServiceError || response.StatusCode == Domain.Enums.StatusCode.NotFound)
            {
                return BadRequest(new { message = "Failed to log in" });;
            }
            HttpContext.Response.Cookies.Append(".Application.DeveloperCode", response.Data,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60)
            });
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = await _userService.Register(model);
            if (response.StatusCode == Domain.Enums.StatusCode.Conflict || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError)
            {
                return BadRequest(new { message = "Failed to register" });
            }
            return Ok(response);
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            if (response.StatusCode == Domain.Enums.StatusCode.NotFound || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError)
            {
                return BadRequest(new { message = "Failed to get users list" });
            }
            return Ok(response);
        }
    }
}
