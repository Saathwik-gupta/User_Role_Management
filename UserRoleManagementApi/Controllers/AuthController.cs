using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserRoleManagementApi.Models;
using UserRoleManagementApi.Services;

namespace UserRoleManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Check if user exists by email or username and validate password
                var user = await _userService.GetUserByEmailForAuthAsync(loginDTO.UserName) 
                           ?? await _userService.GetUserByUsernameForAuthAsync(loginDTO.UserName);

                if (user == null)
                {
                    Console.WriteLine("User not found with the provided email/username.");
                    return Unauthorized("Invalid username or email.");
                }

                if (!VerifyPassword(loginDTO.Password, user.Password))
                {
                    Console.WriteLine("Password verification failed.");
                    return Unauthorized("Invalid password.");
                }

                // Generate a JWT token for the user
                var token = GenerateJwtToken(user);
                Console.WriteLine($"Generated Token: {token}");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                // Log and return an internal server error response
                Console.WriteLine($"Error occurred while authenticating user: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "SuperAdmin") // Default role if null
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"])),
                    Issuer = _configuration["JwtSettings:Issuer"],
                    Audience = _configuration["JwtSettings:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating JWT token: {ex.Message}");
                throw new Exception("Failed to generate authentication token. Please try again.");
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            try
            {
                string hashedInputPassword = PasswordHasher.HashPassword(inputPassword); 
                return hashedInputPassword == storedHash;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying password: {ex.Message}");
                throw new Exception("An error occurred while verifying the password. Please try again.");
            }
        }
    }
}
