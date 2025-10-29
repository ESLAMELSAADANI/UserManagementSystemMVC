using Day06_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Day06_Demo.DAL
{
    public class AuthDemoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public AuthDemoDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected AuthDemoDbContext()
        {
        }
    }
}
