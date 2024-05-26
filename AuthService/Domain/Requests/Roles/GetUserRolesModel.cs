using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Requests.Roles
{
    public class GetUserRolesModel
    {
        [Required]
        public string Username { get; set; }
    }
}
