using Microsoft.EntityFrameworkCore;
namespace UserRoleManagementApi.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> mst_users { get; set; }
        public DbSet<Role> mst_roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeding data for the mst_roles tables
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "SuperAdmin", CreatedBy = "SuperAdmin", CreatedDate = DateTime.UtcNow, IsActive = true }
            );

            // Seeding data for the mst_users table
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    UserName = "superadmin",
                    Email = "superadmin@example.com",
                    RoleID = 1,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = "SuperAdmin",
                    IsActive = true,
                    MobileNumber = "1234567890",
                    Password = PasswordHasher.HashPassword("SuperAdmin@123") // Hashed password
                }        
            );
        }
    }
}
