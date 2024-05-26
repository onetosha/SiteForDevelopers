using AuthService.Domain.Enums;
using AuthService.Domain.Models;
using AuthService.Domain.Requests.Users;
using AuthService.Domain.Responses;
using AuthService.Helpers;
using AuthService.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services
{
    public interface IUserService
    {
        Task<IBaseResponse<string>> Login(LoginModel model);
        Task<IBaseResponse<bool>> Register(RegisterModel model);
        Task<IBaseResponse<List<User>>> GetAllUsers();
    }
    public class UserService : IUserService
    {
        private readonly JWTSettings _options;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IOptions<JWTSettings> options)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _options = options.Value;
        }

        private async Task<string> GetToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userRoleRepository.GetUserRoles(user.Id);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            //var userRoles = await _userManager.GetRolesAsync(user);
            //claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IBaseResponse<string>> Login(LoginModel model)
        {
            var response = new BaseResponse<string>();

            try
            {
                var user = await _userRepository.FindIfExist(model.Username);

                if (user == null || VerifyPasswordHash(model.Password, user.PasswordHash))
                {
                    response.Description = "User not found or incorrect password";
                    response.StatusCode = StatusCode.NotFound;
                    return response;
                }
                var token = await GetToken(user);
                response.Description = "User logged in";
                response.StatusCode = StatusCode.OK;
                response.Data = token;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[Login]: {ex.Message}";
                response.StatusCode = StatusCode.InternalServiceError;
                return response;
            }
        }
        public async Task<IBaseResponse<bool>> Register(RegisterModel model)
        {
            var response = new BaseResponse<bool>();

            try
            {
                if(await _userRepository.FindIfExist(model.Username) != null)
                {
                    response.Description = "Username is already taken!";
                    response.StatusCode = StatusCode.Conflict;
                    return response;
                }
                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = CreatePasswordHash(model.Password),
                    Email = model.Email
                };
                bool result = await _userRepository.Create(user);
                response.Description = "User created";
                response.StatusCode = StatusCode.OK;
                response.Data = result;
                return response;
            }
            catch(Exception ex)
            {
                response.Description = $"[Register]: {ex.Message}";
                response.StatusCode = StatusCode.InternalServiceError;
                return response;
            }
        }
        private static string CreatePasswordHash(string password)
        {
            var hmac = new HMACSHA256();
            var passwordHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            return passwordHash;
        }
        private static bool VerifyPasswordHash(string inputPassword, string passwordHash)
        {
            var hmac = new HMACSHA256();
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(inputPassword)));
            return computedHash == passwordHash;
        }

        public async Task<IBaseResponse<List<User>>> GetAllUsers()
        {
            var response = new BaseResponse<List<User>>();

            try
            {
                var userList = await _userRepository.GetAll();
                if (userList.Count == 0)
                {
                    response.Description = "Users not found";
                    response.StatusCode = StatusCode.NotFound;
                    return response;
                }
                response.Description = $"{userList.Count} users found";
                response.StatusCode = StatusCode.OK;
                response.Data = userList;
                return response;
            }
            catch (Exception ex)
            {
                response.Description = $"[GetAllUser]: {ex.Message}";
                response.StatusCode = StatusCode.InternalServiceError;
                return response;
            }
        }
    }
}
