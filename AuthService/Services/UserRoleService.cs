using AuthService.Domain.Enums;
using AuthService.Domain.Models;
using AuthService.Domain.Requests.Roles;
using AuthService.Domain.Responses;
using AuthService.Repositories.Interfaces;

namespace AuthService.Services
{
    public interface IUserRoleService
    {
        public Task<IBaseResponse<List<Role>>> GetUserRoles(GetUserRolesModel model);
        public Task<IBaseResponse<bool>> AddRoleToUser(UserRoleModel model);
        public Task<IBaseResponse<bool>> RemoveRoleFromUser(UserRoleModel model);
    }
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UserRoleService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository) 
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IBaseResponse<bool>> AddRoleToUser(UserRoleModel model)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var user = await _userRepository.FindIfExist(model.userName);
                if (user == null)
                {
                    response.StatusCode = StatusCode.NotFound;
                    response.Description = "User not found";
                    return response;
                }
                var role = await _roleRepository.FindIfExist(model.roleName);
                if (role == null)
                {
                    response.StatusCode = StatusCode.NotFound;
                    response.Description = "Role not found";
                    return response;
                }
                var userRolesList = await _userRoleRepository.GetUserRoles(user.Id);
                if (userRolesList.Contains(role))
                {
                    response.StatusCode = StatusCode.Conflict;
                    response.Description = "User already have this role";
                    return response;
                }
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
                bool result = await _userRoleRepository.Create(userRole);
                response.Data = result;
                response.Description = "Role added to user";
                response.StatusCode = StatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCode.InternalServiceError;
                response.Description = $"[AddRoleToUser]: {ex.Message}";
                return response;
            }
        }

        public async Task<IBaseResponse<List<Role>>> GetUserRoles(GetUserRolesModel model)
        {
            var response = new BaseResponse<List<Role>>();
            try
            {
                var user = await _userRepository.FindIfExist(model.Username);
                if (user == null)
                {
                    response.StatusCode = StatusCode.NotFound;
                    response.Description = "User not found";
                    return response;
                }
                var userRoles = await _userRoleRepository.GetUserRoles(user.Id);
                response.StatusCode = StatusCode.OK;
                response.Description = $"{userRoles.Count} roles found";
                response.Data = userRoles;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCode.InternalServiceError;
                response.Description = $"[GetUserRoles]: {ex.Message}";
                return response;
            }
        }

        public async Task<IBaseResponse<bool>> RemoveRoleFromUser(UserRoleModel model)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var user = await _userRepository.FindIfExist(model.userName);
                if (user == null)
                {
                    response.StatusCode = StatusCode.NotFound;
                    response.Description = "User not found";
                    return response;
                }
                var role = await _roleRepository.FindIfExist(model.roleName);
                if (role == null)
                {
                    response.StatusCode = StatusCode.NotFound;
                    response.Description = "Role not found";
                    return response;
                }
                var userRole = await _userRoleRepository.Get(user.Id, role.Id);
                if (userRole == null)
                {
                    response.StatusCode = StatusCode.NotFound;
                    response.Description = "User not have this role";
                    return response;
                }
                bool result = await _userRoleRepository.Delete(userRole);
                response.Data = result;
                response.Description = "Role removed from user";
                response.StatusCode = StatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCode.InternalServiceError;
                response.Description = $"[RemoveRoleFromUser]: {ex.Message}";
                return response;
            }
        }
    }
}
