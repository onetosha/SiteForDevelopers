using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Roles
{
    public class EditPostRequest
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public List<string> Roles { get; set; }
    }
}
