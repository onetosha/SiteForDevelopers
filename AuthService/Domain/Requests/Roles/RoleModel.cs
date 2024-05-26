using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Requests.Roles
{
    public class RoleModel
    {
        [Required]
        public string roleName { get; set; }
    }
}
