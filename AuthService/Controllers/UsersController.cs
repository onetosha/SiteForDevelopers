using AuthService.Authorization;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateReqest model)
        {
            var response = _userService.Authenticate(model);
            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
