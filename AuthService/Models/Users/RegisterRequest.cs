using AuthService.Entities;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Users
{
    public class RegisterRequest : LoginReqest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
