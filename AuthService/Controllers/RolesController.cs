using AuthService.Models.Roles;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RolesController : Controller
    {
        IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        
        //Создание ролей
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDeleteRequest model)
        {
            
            var response = await _roleService.Create(model);
            if (response == null)
            {
                return BadRequest(new { message = "Failed to create new Role" });
            }
            return Ok(response);
        } 

        //Удаление ролей
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] CreateDeleteRequest model)
        {
            var response = await _roleService.Delete(model);
            if (response == null)
            {
                return BadRequest(new { message = "Failed to delete Role" });
            }
            return Ok(response);
        }

        [HttpGet("list")]
        public IActionResult UserList()
        {
            return Ok(_roleService.UserList());
        }
        [HttpGet("roles")]
        public IActionResult RoleList()
        {
            return Ok(_roleService.RoleList());
        }

        //Получение списка ролей у пользователя
        [HttpPost("userroles")]
        public async Task<IActionResult> Edit([FromBody] ShowRolesReqest model)
        {
            ChangeRoleModel response = await _roleService.ShowRoles(model);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //Назначение/удаление ролей пользователя
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] EditPostRequest model)
        {
            var response = await _roleService.EditPost(model);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }


}
