using AuthService.Domain.Models;

namespace AuthService.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<bool> Create(UserRole userRole);
        Task<bool> Delete(UserRole userRole);
        Task<UserRole> Get(int userId, int roleId);
        Task<List<Role>> GetUserRoles(int userId);
    }
}
