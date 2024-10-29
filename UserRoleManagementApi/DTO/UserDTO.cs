using System.ComponentModel.DataAnnotations;

public class UserDTO
{
    public int UserID { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string Email { get; set; }

    public string Password { get; set; }

    public int RoleID { get; set; }

    public string RoleName { get; set; }  // Included for easier role lookup/display

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public bool IsActive { get; set; }

    [Phone(ErrorMessage = "Invalid mobile number format")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string MobileNumber { get; set; }
}
