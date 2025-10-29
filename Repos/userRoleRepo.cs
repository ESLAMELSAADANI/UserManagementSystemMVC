using Day06_Demo.DAL;
using Day06_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Day06_Demo.Repos
{
    public interface IUserRoleRepo : IEntityRepo<UserRole>
    {
        public Task AddAsync(int userId, int roleId);
        public Task<UserRole> GetAsync(int userId, int roleId);
    }
    public class userRoleRepo : EntityRepo<UserRole>, IUserRoleRepo
    {
        public userRoleRepo(AuthDemoDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task AddAsync(int userId, int roleId)
        {
            var model = new UserRole()
            {
                UserId = userId,
                RoleId = roleId
            };
            await dbContext.UserRoles.AddAsync(model);
        }

        public Task<UserRole> GetAsync(int userId, int roleId)
        {
            return dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == roleId && ur.UserId == userId);
        }
    }
}
