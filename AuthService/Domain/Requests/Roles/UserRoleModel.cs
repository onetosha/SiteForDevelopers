using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Requests.Roles
{
    public class UserRoleModel
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string roleName { get; set; }
    }
}
