using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserRoleManagementApi.Migrations;
using UserRoleManagementApi.Models;
using UserRoleManagementApi.Services;

namespace UserRoleManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authorization for all endpoints
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin")] 
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(new {message ="successfully retrieved users."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retreiving the Users: {ex.Message}");
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin,Admin")] 
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} is not found.");
                }
                return Ok(new{message = "successfully retrived user with ID."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving the user by Id: {ex.Message}");
            }
        }

        // POST: api/User
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Admin")] 
        public async Task<ActionResult> AddUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.AddUserAsync(userDTO);
                return Ok(new { message = "Successfully added new user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while adding a new user: {ex.Message}");
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")] 
        public async Task<ActionResult<UserDTO>> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (id != userDTO.UserID)
            {
                return BadRequest("User ID mismatch.");
            }

            try
            {
                // Fetch the existing user details
                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                // Update the user
                await _userService.UpdateUserAsync(userDTO);
                var updatedUser = await _userService.GetUserByIdAsync(id);
                return Ok(new{message = "User is updated successfully."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while updating the user: {ex.Message}");
            }
        }


        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")] 
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} is not found.");
                }

                await _userService.DeleteUserAsync(id);
                return Ok(new { message = "Successfully deleted the User(Soft deleted)." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while deleting the user: {ex.Message}");
            }
        }

        // GET: api/User/role/5
        [HttpGet("role/{roleId}")]
        [Authorize(Roles = "SuperAdmin,Admin")] 
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByRole(int roleId)
        {
            try
            {
                var users = await _userService.GetUsersByRoleAsync(roleId);
                return Ok(new{message = "Successfully fetch the Users by Role."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving the Users by Role: {ex.Message}");
            }
        }

        // GET: api/User/email/{email}
        [HttpGet("email/{email}")]
        [Authorize(Roles = "SuperAdmin,Admin")] 
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailForAuthAsync(email);
                if (user == null)
                {
                    return NotFound($"User with email {email} is not found.");
                }
                return Ok(new{message = "Successfully fetch the User by Email."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving the User by Email: {ex.Message}");
            }
        }
    }
}
