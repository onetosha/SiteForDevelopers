using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Roles
{
    public class CreateDeleteRequest
    {
        [Required]
        public string roleName { get; set; }
    }
}
