using UserRoleManagementApi.Models;
using UserRoleManagementApi.Repositories;
using System;

namespace UserRoleManagementApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllRolesAsync();
                return roles.Select(role => MapToRoleDTO(role));
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                throw new ApplicationException("Error fetching roles. Please try again later.", ex);
            }
        }

        public async Task<RoleDTO> GetRoleByIdAsync(int id)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with ID {id} not found.");
                }
                return MapToRoleDTO(role);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching the role by ID. Please try again.", ex);
            }
        }

        public async Task AddRoleAsync(RoleDTO roleDTO)
        {
            try
            {
                if (roleDTO == null)
                {
                    throw new ArgumentNullException(nameof(roleDTO), "RoleDTO cannot be null.");
                }

                var existingRole = await _roleRepository.GetRoleByNameAsync(roleDTO.RoleName);
                if (existingRole != null)
                {
                    throw new InvalidOperationException("A role with this name already exists.");
                }

                var role = new Role
                {
                    RoleName = roleDTO.RoleName,
                    CreatedBy = roleDTO.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                await _roleRepository.AddRoleAsync(role);
            }
            catch (ArgumentNullException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the role. Please try again.", ex);
            }
        }

        public async Task UpdateRoleAsync(RoleDTO roleDTO)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(roleDTO.RoleID);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with ID {roleDTO.RoleID} not found.");
                }

                role.RoleName = roleDTO.RoleName;
                role.IsActive = roleDTO.IsActive;

                await _roleRepository.UpdateRoleAsync(role);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the role. Please try again.", ex);
            }
        }

        public async Task DeleteRoleAsync(int id)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with ID {id} not found.");
                }

                await _roleRepository.DeleteRoleAsync(id);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the role. Please try again.", ex);
            }
        }

        public async Task<RoleDTO> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var role = await _roleRepository.GetRoleByNameAsync(roleName);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with name '{roleName}' not found.");
                }
                return MapToRoleDTO(role);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ApplicationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching the role by name. Please try again.", ex);
            }
        }

        private RoleDTO MapToRoleDTO(Role role)
        {
            return new RoleDTO
            {
                RoleID = role.RoleID,
                RoleName = role.RoleName,
                CreatedBy = role.CreatedBy,
                CreatedDate = role.CreatedDate,
                IsActive = role.IsActive
            };
        }
    }
}
