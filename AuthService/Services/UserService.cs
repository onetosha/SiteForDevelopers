using AuthService.Entities;
using AuthService.Helpers;
using AuthService.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public interface IUserService
    {
        Task<string> Login(LoginReqest model);
        Task<string> Register(RegisterRequest model);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _options;
        public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JWTSettings> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = options.Value;
        }
        public async Task<string> Login(LoginReqest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                IEnumerable<Claim> claims = await _userManager.GetClaimsAsync(user);
                var token = GetToken(user, claims);
                return token;
            }
            return null;
        }
        public async Task<string> Register(RegisterRequest model)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("Role", model.Role.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, model.Email));

                await _userManager.AddClaimsAsync(user, claims);
                return "Usr created";
            }
            else
            {
                return "Usr not created";
            }
        }
        private string GetToken(IdentityUser user, IEnumerable<Claim> principal)
        {
            var claims = principal.ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
