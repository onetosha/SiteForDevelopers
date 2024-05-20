﻿using AuthService.Models.Users;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthService.Controllers
{
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
        public async Task<IActionResult> Login([FromBody] LoginReqest model)
        {
            var response = await _userService.Login(model);
            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            HttpContext.Response.Cookies.Append(".Application.DeveloperCode", response,
            new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(60)
            });
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var response = await _userService.Register(model);
            if (response == null)
            {
                return BadRequest(new { message = "Data is incorrect" });
            }
            return Ok(response);
        }
    }
}
