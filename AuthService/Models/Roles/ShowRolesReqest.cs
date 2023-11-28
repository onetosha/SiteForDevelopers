using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Roles
{
    public class ShowRolesReqest
    {
        [Required]
        public string userName { get; set; }
    }
}
