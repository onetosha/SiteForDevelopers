using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Users
{
    public class RegisterRequest : LoginReqest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
