using AuthService.Authorization;
using AuthService.Entities;
using AuthService.Models;

namespace AuthService.Services
{
    public interface IUserService
    {
        AuthenticateResponse? Authenticate(AuthenticateReqest model);
        IEnumerable<User> GetAll();
        User? GetById(int id);
    }
    public class UserService : IUserService
    {
        public List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };
        private readonly IJwtUtils _jwtUtils;
        public UserService(IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
        }
        public AuthenticateResponse? Authenticate(AuthenticateReqest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (user == null)
            {
                return null;
            }

            var token = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User? GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }
    }
}
