using System.ComponentModel.DataAnnotations;

public class RoleDTO
{
    public int RoleID { get; set; }

    [Required(ErrorMessage = "RoleName is required")]
    [StringLength(50, ErrorMessage = "RoleName cannot exceed 50 characters")]
    public string RoleName { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public bool IsActive { get; set; }
}
