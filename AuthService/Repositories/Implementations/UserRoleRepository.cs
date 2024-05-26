using AuthService.Domain.Models;
using AuthService.Helpers;
using AuthService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories.Implementations
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDBContext _dbContext;
        public UserRoleRepository(AppDBContext dBContext) 
        {
            _dbContext = dBContext;
        }
        public async Task<bool> Create(UserRole userRole)
        {
            await _dbContext.UsersRoles.AddAsync(userRole);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(UserRole userRole)
        {
            _dbContext.UsersRoles.Remove(userRole);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserRole> Get(int userId, int roleId)
        {
            return await _dbContext.UsersRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);
        }

        public async Task<List<Role>> GetUserRoles(int userId)
        {
            return await _dbContext.UsersRoles.Where(x => x.UserId == userId).Select(x => x.Role).ToListAsync();
        }
    }
}
