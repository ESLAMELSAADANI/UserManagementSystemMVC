using Day06_Demo.DAL;
using Day06_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Day06_Demo.Repos
{
    public interface IRole : IEntityRepo<Role>
    {
        public bool IsRoleExist(string roleName);
        public Task<Role> GetByNameAsync(string roleName);
        public Task<List<Role>> GetUserRolesAsync(int userID);
        public Task<List<Role>> GetUserAnotherRolesAsync(int userID);

    }
    public class roleRepo : EntityRepo<Role>, IRole
    {
        public roleRepo(AuthDemoDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(ur => ur.RoleName.ToLower() == roleName);
        }

        public Task<List<Role>> GetUserAnotherRolesAsync(int userID)
        {
            return dbContext.Roles.Include(r => r.UserRoles).Where(r => !r.UserRoles.Any(ur => ur.UserId == userID)).ToListAsync();
        }

        public Task<List<Role>> GetUserRolesAsync(int userID)
        {
            return dbContext.Roles.Include(r => r.UserRoles).Where(r => r.UserRoles.Any(ur => ur.UserId == userID)).ToListAsync();
        }

        public bool IsRoleExist(string roleName)
        {
            return dbContext.Roles.Any(r => r.RoleName == roleName);
        }
    }
}
