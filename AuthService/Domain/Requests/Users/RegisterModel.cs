using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Requests.Users
{
    public class RegisterModel : LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
