using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserRoleManagementApi.Models
{
    public class User
    {
        [Key]  // Primary Key
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [ForeignKey("Role")]  // Foreign key to Role table
        public int RoleID { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;  // Default to current date/time

        [Required(ErrorMessage = "CreatedBy is required")]
        [StringLength(50, ErrorMessage = "CreatedBy cannot exceed 50 characters")]
        public string CreatedBy { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;  // Default to active

        [Phone(ErrorMessage = "Invalid mobile number format")]
        public string MobileNumber { get; set; }  // Optional field

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 4 characters long", MinimumLength = 4)]
        public string Password { get; set; }  // Store hashed password

        // Navigation property for the foreign key relationship with Role
        public virtual Role Role { get; set; }
    }
}
