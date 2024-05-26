using AuthService.Domain.Models;

namespace AuthService.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<bool> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        Task<T> FindIfExist(string name);
    }
}
