using System.ComponentModel.DataAnnotations;

namespace UserRoleManagementApi.Models
{
    public class Role
    {
        [Key]  // Primary Key
        public int RoleID { get; set; }

        [Required(ErrorMessage = "RoleName is required")]
        [StringLength(50, ErrorMessage = "RoleName cannot exceed 50 characters")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "CreatedBy is required")]
        [StringLength(50, ErrorMessage = "CreatedBy cannot exceed 50 characters")]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;  // Default to current date/time

        [Required]
        public bool IsActive { get; set; } = true;  // Default to active

        // Navigation property for users assigned to this role
        public virtual ICollection<User> Users { get; set; }
    }
}
