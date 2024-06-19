using AuthService.Domain.Requests.Roles;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [Route("[controller]")] 
    //TODO: RESPONSES
    public class RolesController : Controller
    {
        IRoleService _roleService;
        IUserRoleService _userRoleService;
        public RolesController(IRoleService roleService, IUserRoleService userRoleService)
        {
            _roleService = roleService;
            _userRoleService = userRoleService;
        }
        
        //Создание ролей
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RoleModel model)
        {
            
            var response = await _roleService.CreateRole(model);
            if (response.StatusCode == Domain.Enums.StatusCode.Conflict || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError)
            {
                return BadRequest(new { message = "Failed to create new Role" });
            }
            return Ok(response);
        } 

        //Удаление ролей
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] RoleModel model)
        {
            var response = await _roleService.DeleteRole(model);
            if (response.StatusCode == Domain.Enums.StatusCode.NotFound || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError)
            {
                return BadRequest(new { message = "Failed to delete Role" });
            }
            return Ok(response);
        }
        [HttpGet("list")]
        public async Task<IActionResult> RoleList()
        {
            var response = await _roleService.GetAllRoles();
            if (response.StatusCode == Domain.Enums.StatusCode.NotFound || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError)
            {
                return BadRequest(new { message = "Failed to get roles list" });
            }
            return Ok(response);
        }

        //Получение списка ролей у пользователя
        [HttpPost("user")]
        public async Task<IActionResult> GetUserRoles([FromBody] GetUserRolesModel model)
        {
            var response = await _userRoleService.GetUserRoles(model);
            if (response.StatusCode == Domain.Enums.StatusCode.NotFound || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError)
            {
                return BadRequest(new { message = "Failed to get user roles" });
            }
            return Ok(response);
        }

        //Добавление роли пользователю
        [HttpPost("add")]
        public async Task<IActionResult> AddRoleToUser([FromBody] UserRoleModel model)
        {
            var response = await _userRoleService.AddRoleToUser(model);
            if (response.StatusCode == Domain.Enums.StatusCode.NotFound || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError || response.StatusCode == Domain.Enums.StatusCode.Conflict)
            {
                return BadRequest(new { message = "Failed to add role to user" });
            }
            return Ok(response);
        }
        //Удаление роли у пользователю
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserRoleModel model)
        {
            var response = await _userRoleService.RemoveRoleFromUser(model);
            if (response.StatusCode == Domain.Enums.StatusCode.NotFound || response.StatusCode == Domain.Enums.StatusCode.InternalServiceError)
            {
                return BadRequest(new { message = "Failed to remove role from user" });
            }
            return Ok(response);
        }
    }


}
