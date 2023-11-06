using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }

    }
}
