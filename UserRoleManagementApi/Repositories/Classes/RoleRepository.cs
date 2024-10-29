// RoleRepository.cs
using Microsoft.EntityFrameworkCore;
using UserRoleManagementApi.Models;

namespace UserRoleManagementApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserContext _context;

        public RoleRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            // Fetch only active roles
            return await _context.mst_roles
                .Where(r => r.IsActive) 
                .ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            var role = await _context.mst_roles.FindAsync(id);
            return role; 
        }


        public async Task AddRoleAsync(Role role)
        {
            await _context.mst_roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            var existingRole = await _context.mst_roles.FindAsync(role.RoleID);
            if (existingRole != null)
            {
                existingRole.RoleName = role.RoleName;
                existingRole.IsActive = role.IsActive; 
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteRoleAsync(int id)
        {
            // Mark the role as inactive (soft delete)
            var role = await _context.mst_roles.FindAsync(id);
            if (role != null)
            {
                role.IsActive = false; 
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.mst_roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName && r.IsActive); 
        }
    }
}
