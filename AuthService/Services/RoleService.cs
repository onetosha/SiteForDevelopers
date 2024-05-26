using AuthService.Domain.Models;
using AuthService.Domain.Requests.Roles;
using AuthService.Domain.Responses;
using AuthService.Repositories.Interfaces;

namespace AuthService.Services
{
    public interface IRoleService
    {
        public Task<IBaseResponse<bool>> CreateRole(RoleModel model);
        public Task<IBaseResponse<bool>> DeleteRole(RoleModel model);
        public Task<IBaseResponse<List<Role>>> GetAllRoles();
        //public Task<IBaseResponse<ChangeRoleModel>> ShowUserRoles(ShowUserRolesModel model);
        //public Task<string> EditPost(EditPostRequest model);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<IBaseResponse<bool>> CreateRole(RoleModel model)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var role = await _roleRepository.FindIfExist(model.roleName);
                if (role != null)
                {
                    response.Description = "Role is already exist";
                    response.StatusCode = Domain.Enums.StatusCode.Conflict;
                }
                var newRole = new Role
                {
                    Name = model.roleName
                };
                bool result = await _roleRepository.Create(newRole);
                response.StatusCode = Domain.Enums.StatusCode.OK;
                response.Description = "Role created";
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[CreateRole]: {ex.Message}";
                response.StatusCode = Domain.Enums.StatusCode.InternalServiceError;
                return response;
            }
        }

        public async Task<IBaseResponse<bool>> DeleteRole(RoleModel model)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var role = await _roleRepository.FindIfExist(model.roleName);
                if (role == null)
                {
                    response.Description = "Role not found";
                    response.StatusCode = Domain.Enums.StatusCode.NotFound;
                }
                bool result = await _roleRepository.Delete(role);
                response.StatusCode = Domain.Enums.StatusCode.OK;
                response.Description = "Role deleted";
                response.Data = result;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[DeleteRole]: {ex.Message}";
                response.StatusCode = Domain.Enums.StatusCode.InternalServiceError;
                return response;
            }
        }

        public async Task<IBaseResponse<List<Role>>> GetAllRoles()
        {
            var response = new BaseResponse<List<Role>>();
            try
            {
                var rolesList = await _roleRepository.GetAll();
                if (rolesList.Count == 0)
                {
                    response.Description = "Roles not found";
                    response.StatusCode = Domain.Enums.StatusCode.NotFound;
                }
                response.StatusCode = Domain.Enums.StatusCode.OK;
                response.Description = $"{rolesList.Count} roles found";
                response.Data = rolesList;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[GetAllRoles]: {ex.Message}";
                response.StatusCode = Domain.Enums.StatusCode.InternalServiceError;
                return response;
            }
        }

        //public async Task<string> EditPost(EditPostRequest model)
        //{
        //    List<string> roles = model.Roles;
        //    IdentityUser user = await _userManager.FindByNameAsync(model.userName);
        //    if (user != null)
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);
        //        var addedRoles = roles.Except(userRoles);
        //        var removedRoles = userRoles.Except(roles);

        //        await _userManager.AddToRolesAsync(user, addedRoles);

        //        await _userManager.RemoveFromRolesAsync(user, removedRoles);

        //        return $"Список ролей пользователя '{model.userName}' успешно изменен!";
        //    }
        //    return null;
        //}
        //public List<IdentityRole> RoleList()
        //{
        //    return _roleManager.Roles.ToList();
        //}
    }
}
