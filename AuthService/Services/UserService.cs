using AuthService.Authorization;
using AuthService.Entities;
using AuthService.Helpers;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

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
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                throw new AppException("Username or password are incorrect");
            }
            var token = _jwtUtils.GenerateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }
        public AuthenticateResponse? Registration(RegistrationRequest model)
        {
            User user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role
            };
            if (user == null) 
            {
                throw new AppException("Failed to create new user");
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
            var user = _dbContext.user.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
