using AuthService.Models.Roles;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RolesController : Controller
    {
        IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        //Создание ролей
        [HttpPost("Create")]
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
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromBody] CreateDeleteRequest model)
        {
            var response = await _roleService.Delete(model);
            if (response == null)
            {
                return BadRequest(new { message = "Failed to delete Role" });
            }
            return Ok(response);
        }

        //Проверка работы ролей
        [Authorize(Roles = "Admin")]
        [HttpGet("UserList")]
        public IActionResult UserList()
        {
            return Ok(_roleService.UserList());
        }

        //Получение списка ролей у пользователя
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit([FromBody] EditGetRequest model)
        {
            ChangeRoleModel response = await _roleService.EditGet(model);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //Назначение/удаление ролей пользователя
        [HttpPost("Edit")]
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
