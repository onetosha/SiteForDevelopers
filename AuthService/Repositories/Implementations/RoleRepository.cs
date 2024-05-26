using AuthService.Domain.Models;
using AuthService.Helpers;
using AuthService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDBContext _dbContext;
        public RoleRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(Role entity)
        {
            await _dbContext.Roles.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Role entity)
        {
            _dbContext.Roles.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Role> FindIfExist(string name)
        {
            return await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Role> Get(int id)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Role>> GetAll()
        {
            return await _dbContext.Roles.ToListAsync();
        }

        public async  Task<Role> Update(Role entity)
        {
            _dbContext.Roles.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
