using UserRoleManagementApi.Models;

namespace UserRoleManagementApi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task AddUserAsync(UserDTO userDTO);
        Task UpdateUserAsync(UserDTO userDTO);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(int roleId);
        
        // Authentication methods
        Task<User> GetUserByEmailForAuthAsync(string email);
        Task<User> GetUserByUsernameForAuthAsync(string username);
    }
}
