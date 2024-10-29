using System.ComponentModel.DataAnnotations;

namespace UserRoleManagementApi.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters long")]
        public string Password { get; set; }
    }
}
