using Microsoft.EntityFrameworkCore;
using UserRoleManagementApi.Models;

namespace UserRoleManagementApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // Fetch only active users
            return await _context.mst_users
                            .Where(u => u.IsActive)
                            .OrderByDescending(u => u.CreatedDate)
                            .Include(u => u.Role)
                            .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            // Fetch user by ID, ensure it is active
            return await _context.mst_users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserID == userId); 
        }

        public async Task AddUserAsync(User user)
        {
            await _context.mst_users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            // Allow updating regardless of IsActive state
            var existingUser = await _context.mst_users.FindAsync(user.UserID);
            if (existingUser != null)
            {
                // Update properties
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.IsActive = user.IsActive; 
                existingUser.CreatedDate = user.CreatedDate; 
                // Add more properties as needed

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            // Mark user as inactive (soft delete)
            var user = await _context.mst_users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = false; 
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await _context.mst_users
                .Where(u => u.RoleID == roleId && u.IsActive) 
                .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.mst_users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(u => u.Email == email  && u.IsActive); 
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.mst_users
                                .Include(u => u.Role)
                                .FirstOrDefaultAsync(u => u.UserName == username  && u.IsActive); 
        }
    }
}
