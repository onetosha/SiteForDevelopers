//using AuthService.Models.Roles;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace AuthService.Controllers
//{
//    [ApiController]
//    [Authorize]
//    [Route("[controller]")]
//    public class RolesController : Controller
//    {
//        RoleManager<IdentityRole> _roleManager;
//        UserManager<IdentityUser> _userManager;
//        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create(string roleName)
//        {
//            if (!string.IsNullOrEmpty(roleName))
//            {
//                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(roleName));
//                if (result.Succeeded)
//                {
//                    return Ok(roleName);
//                }
//            }
//            return BadRequest();
//        }
//        [HttpPost("Delete")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            IdentityRole role = await _roleManager.FindByIdAsync(id);
//            if (role != null)
//            {
//                IdentityResult result = await _roleManager.DeleteAsync(role);
//            }
//            return Ok(id);
//        }
//        [HttpGet("UserList")]
//        public IActionResult UserList() => Ok(_userManager.Users.ToList());
//        [HttpGet("Edit")]
//        public async Task<IActionResult> Edit(string userId)
//        {
//            // получаем пользователя
//            IdentityUser user = await _userManager.FindByIdAsync(userId);
//            if (user != null)
//            {
//                // получем список ролей пользователя
//                var userRoles = await _userManager.GetRolesAsync(user);
//                var allRoles = _roleManager.Roles.ToList();
//                ChangeRoleViewModel model = new ChangeRoleViewModel
//                {
//                    UserId = user.Id,
//                    UserEmail = user.Email,
//                    UserRoles = userRoles,
//                    AllRoles = allRoles
//                };
//                return Ok(model);
//            }

//            return NotFound();
//        }
//        [HttpPost("Edit")]
//        public async Task<IActionResult> Edit(string userId, List<string> roles)
//        {
//            IdentityUser user = await _userManager.FindByIdAsync(userId);
//            if (user != null)
//            {
//                var userRoles = await _userManager.GetRolesAsync(user);
//                var allRoles = _roleManager.Roles.ToList();
//                var addedRoles = roles.Except(userRoles);
//                var removedRoles = userRoles.Except(roles);

//                await _userManager.AddToRolesAsync(user, addedRoles);

//                await _userManager.RemoveFromRolesAsync(user, removedRoles);

//                return Ok(userId);
//            }

//            return NotFound();
//        }
//    }


//}
