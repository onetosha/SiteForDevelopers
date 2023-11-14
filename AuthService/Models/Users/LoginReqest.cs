using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Users
{
    public class LoginReqest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
