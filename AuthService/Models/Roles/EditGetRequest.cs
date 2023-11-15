using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Roles
{
    public class EditGetRequest
    {
        [Required]
        public string userName { get; set; }
    }
}
