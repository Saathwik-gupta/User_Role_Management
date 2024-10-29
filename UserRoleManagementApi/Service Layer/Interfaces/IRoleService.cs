namespace UserRoleManagementApi.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int id);
        Task AddRoleAsync(RoleDTO roleDTO);
        Task UpdateRoleAsync(RoleDTO roleDTO);
        Task DeleteRoleAsync(int id);
        Task<RoleDTO> GetRoleByNameAsync(string roleName);
    }
}
