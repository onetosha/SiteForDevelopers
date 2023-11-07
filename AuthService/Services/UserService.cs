using AuthService.Authorization;
using AuthService.Entities;
using AuthService.Helpers;
using AuthService.Models;

namespace AuthService.Services
{
    public interface IUserService
    {
        AuthenticateResponse? Authenticate(AuthenticateReqest model);
        AuthenticateResponse? Registration(RegistrationRequest model);
        IEnumerable<User> GetAll();
        User? GetById(int id);
    }
    public class UserService : IUserService
    {
        private AuthDBContext _dbContext;
        private readonly IJwtUtils _jwtUtils;
        public UserService(IJwtUtils jwtUtils, AuthDBContext dBContext)
        {
            _dbContext = dBContext;
            _jwtUtils = jwtUtils;
        }
        public AuthenticateResponse? Authenticate(AuthenticateReqest model)
        {
            var user = _dbContext.user.SingleOrDefault(x => x.Username == model.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return null;
            }
            var token = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }
        public AuthenticateResponse? Registration(RegistrationRequest model)
        {
            User user = new User
            {
                Username = model.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            if (user == null) 
            {
                return null;
            }
            _dbContext.user.Add(user);
            _dbContext.SaveChanges();
            var token = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.user;
        }

        public User? GetById(int id)
        {
            return _dbContext.user.FirstOrDefault(x => x.Id == id);
        }
    }
}
