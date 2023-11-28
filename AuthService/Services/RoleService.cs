using AuthService.Models.Roles;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public interface IRoleService
    {
        public Task<string> Create(CreateDeleteRequest model);
        public Task<string> Delete(CreateDeleteRequest model);
        public List<IdentityUser> UserList();
        public Task<ChangeRoleModel> ShowRoles(ShowRolesReqest model);
        public Task<string> EditPost(EditPostRequest model);
    }

    public class RoleService : IRoleService
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<string> Create(CreateDeleteRequest model)
        {
            if (!string.IsNullOrEmpty(model.roleName))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(model.roleName));
                if (result.Succeeded)
                {
                    return $"Роль '{model.roleName}' успешно создана!";
                }
            }
            return null;
        }

        public async Task<string> Delete(CreateDeleteRequest model)
        {
            IdentityRole role = await _roleManager.FindByNameAsync(model.roleName);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return $"Роль '{model.roleName}' успешно удалена!";
        }

        public async Task<ChangeRoleModel> ShowRoles(ShowRolesReqest model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(model.userName);
            if (user != null)
            {
                //Получение списка ролей
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleModel rModel = new ChangeRoleModel
                {
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return rModel;
            }

            return null;
        }

        public async Task<string> EditPost(EditPostRequest model)
        {
            List<string> roles = model.Roles;
            IdentityUser user = await _userManager.FindByNameAsync(model.userName);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return $"Список ролей пользователя '{model.userName}' успешно изменен!";
            }
            return null;
        }

        public List<IdentityUser> UserList()
        {
            return _userManager.Users.ToList();
        }
    }
}
