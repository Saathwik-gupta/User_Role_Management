using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserRoleManagementApi.Models;
using UserRoleManagementApi.Repositories;

namespace UserRoleManagementApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly EmailService _emailService;

        public UserService(IUserRepository userRepository, EmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        // Fetching all users
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                return users.Select(user => MapToUserDTO(user));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all users.", ex);
            }
        }

        // Fetching a user by ID (including inactive)
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                return user == null ? null : MapToUserDTO(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the user with ID {id}.", ex);
            }
        }

        // Adding a new user
        public async Task AddUserAsync(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ArgumentNullException(nameof(userDTO), "UserDTO cannot be null.");
            }

            try
            {
                // Check if a user with the same email or username already exists
                var existingUserByEmail = await _userRepository.GetUserByEmailAsync(userDTO.Email);
                var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(userDTO.UserName);

                if (existingUserByEmail != null)
                {
                    throw new InvalidOperationException("A user with this email already exists.");
                }

                if (existingUserByUsername != null)
                {
                    throw new InvalidOperationException("A user with this username already exists.");
                }

                // Hash the password
                var hashedPassword = PasswordHasher.HashPassword(userDTO.Password);

                // Create the new User object
                var user = new User
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    RoleID = userDTO.RoleID,
                    Password = hashedPassword,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = userDTO.CreatedBy,
                    IsActive = true,
                    MobileNumber = userDTO.MobileNumber
                };

                // Save the new user to the database
                await _userRepository.AddUserAsync(user);

                // Send email notification
                var subject = "User-Email Verification";
                var body = $"<html><body>" +
                            $"Hello {user.UserName},<br/><br/>" +
                            $"Your account has been created successfully.<br/>" +
                            $"Email: {user.Email}<br/>" +
                            $"Password: {user.Password}<br/>" +
                            $"Please log in at your earliest convenience.<br/><br/>" +
                            "Thanks & Regards,<br/>" +
                            "Admin" +
                            $"</body></html>";

                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Failed to add user due to duplicate information.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user.", ex);
            }
        }

        // Updating user details
        public async Task UpdateUserAsync(UserDTO userDTO)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userDTO.UserID);
                if (user == null)
                {
                    throw new KeyNotFoundException($"User with ID {userDTO.UserID} not found.");
                }

                user.UserName = userDTO.UserName;
                user.Email = userDTO.Email;
                user.RoleID = userDTO.RoleID;
                user.IsActive = userDTO.IsActive;
                user.MobileNumber = userDTO.MobileNumber;

                await _userRepository.UpdateUserAsync(user);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("Failed to update user as the user was not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        // Deleting a user (soft delete)
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the user with ID {id}.", ex);
            }
        }

        // Fetching users by role
        public async Task<IEnumerable<UserDTO>> GetUsersByRoleAsync(int roleId)
        {
            try
            {
                var users = await _userRepository.GetUsersByRoleAsync(roleId);
                return users.Select(user => MapToUserDTO(user));
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving users with role ID {roleId}.", ex);
            }
        }

        // Get user by email for authentication
        public async Task<User> GetUserByEmailForAuthAsync(string email)
        {
            try
            {
                return await _userRepository.GetUserByEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user by email for authentication.", ex);
            }
        }

        // Get user by username for authentication
        public async Task<User> GetUserByUsernameForAuthAsync(string username)
        {
            try
            {
                return await _userRepository.GetUserByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user by username for authentication.", ex);
            }
        }

        // Mapping User domain model to UserDTO
        private UserDTO MapToUserDTO(User user)
        {
            try
            {
                return new UserDTO
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleID = user.RoleID,
                    RoleName = user.Role?.RoleName,  // Assuming Role navigation property is loaded
                    CreatedDate = user.CreatedDate,
                    CreatedBy = user.CreatedBy,
                    IsActive = user.IsActive,
                    MobileNumber = user.MobileNumber
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while mapping user to UserDTO.", ex);
            }
        }
    }
}
