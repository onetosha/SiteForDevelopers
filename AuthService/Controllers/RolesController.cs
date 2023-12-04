using AuthService.Models.Roles;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RolesController : Controller
    {
        IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;
        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        //Создание ролей
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDeleteRequest model)
        {
            var response = await _roleService.Create(model);
            if (response == null)
            {
                _logger.LogError($"Ошибка при создании роли {model.roleName}");
                return BadRequest(new { message = "Failed to create new Role" });
            }
            _logger.LogInformation($"Создана роль {model.roleName}");
            return Ok(response);
        } 

        //Удаление ролей
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] CreateDeleteRequest model)
        {
            var response = await _roleService.Delete(model);
            if (response == null)
            {
                _logger.LogError("Ошибка при удалении роли {model.roleName} пользователем {username}", model.roleName, User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return BadRequest(new { message = "Failed to delete Role" });
            }
            _logger.LogInformation("Удалена роль {model.roleName} пользователем {username}", model.roleName, User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(response);
        }

        [HttpGet("list")]
        public IActionResult UserList()
        {
            _logger.LogError("Тестирование из метода UserList пользователем {username}", User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _logger.LogInformation("Попытка получения списка пользователей {username}", User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(_roleService.UserList());
        }

        //Получение списка ролей у пользователя
        [HttpPost("userroles")]
        public async Task<IActionResult> Edit([FromBody] ShowRolesReqest model)
        {
            ChangeRoleModel response = await _roleService.ShowRoles(model);
            if (response == null)
            {
                _logger.LogError($"Ошибка при получении списка ролей для пользователя {model.userName}");
                return NotFound();
            }
            _logger.LogInformation($"Получение списка ролей для пользователя {model.userName}");
            return Ok(response);
        }

        //Назначение/удаление ролей пользователя
        [HttpPost("edit")]
        public async Task<IActionResult> Edit([FromBody] EditPostRequest model)
        {
            var response = await _roleService.EditPost(model);
            if (response == null)
            {
                _logger.LogError($"Ошибка при редактировании ролей для пользователя {model.userName}");
                return NotFound();
            }
            _logger.LogInformation($"Редактирование ролей пользователя {model.userName}");
            return Ok(response);
        }
    }


}
