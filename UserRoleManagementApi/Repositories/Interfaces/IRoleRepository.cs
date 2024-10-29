using UserRoleManagementApi.Models;


namespace UserRoleManagementApi.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int id);
        Task<Role> GetRoleByNameAsync(string roleName);
    }
}
