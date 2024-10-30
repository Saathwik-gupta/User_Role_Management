using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRoleManagementApi.Services;

namespace UserRoleManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply role-based authorization at the controller level
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
         public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/Role
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving roles: {ex.Message}");
            }
        }

        // GET: api/Role/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<RoleDTO>> GetRoleById(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound($"Role with ID {id} is  not found.");
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving role by ID: {ex.Message}");
            }
        }

        // POST: api/Role
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> AddRole([FromBody] RoleDTO roleDTO)
        {
            try
            {
                if (roleDTO == null)
                {
                    return BadRequest("Role data is required.");
                }

                await _roleService.AddRoleAsync(roleDTO);
                return Ok(new{message = "Successfully added new Role."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while adding a new role: {ex.Message}");
            }
        }

        // PUT: api/Role/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> UpdateRole(int id, [FromBody] RoleDTO roleDTO)
        {
            try
            {
                if (roleDTO == null || roleDTO.RoleID != id)
                {
                    return BadRequest("Role data is invalid or does not match the provided ID.");
                }

                var existingRole = await _roleService.GetRoleByIdAsync(id);
                if (existingRole == null)
                {
                    return NotFound($"Role with ID {id} is not found.");
                }

                await _roleService.UpdateRoleAsync(roleDTO);
                return Ok(new{message = "Successfully updated Role."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while updating role: {ex.Message}");
            }
        }

        // DELETE: api/Role/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                var existingRole = await _roleService.GetRoleByIdAsync(id);
                if (existingRole == null)
                {
                    return NotFound($"Role with ID {id} is not found.");
                }

                await _roleService.DeleteRoleAsync(id);
                return Ok(new{message = "Successfully deleted Role by ID."});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while deleting role: {ex.Message}");
            }
        }

        // GET: api/Role/name/{roleName}
        [HttpGet("name/{roleName}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<RoleDTO>> GetRoleByName(string roleName)
        {
            try
            {
                var role = await _roleService.GetRoleByNameAsync(roleName);
                if (role == null)
                {
                    return NotFound($"Role with name '{roleName}' is not found.");
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving role by name: {ex.Message}");
            }
        }
    }
}
