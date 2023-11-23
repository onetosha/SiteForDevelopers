using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Users
{
    public class LoginReqest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
