using Microsoft.AspNetCore.Identity;

namespace AuthService.Models.Roles
{
    public class ChangeRoleModel
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
