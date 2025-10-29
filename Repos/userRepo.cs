using Day06_Demo.DAL;
using Day06_Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Day06_Demo.Repos
{
    public interface IUserRepoExtra : IEntityRepo<User>
    {
        public bool emailExist(string email);
        public bool userNameExist(string userName);
        public Task<User> GetUserByIdWithRolesAsync(int id);
        public Task<User> GetUserByEmailPasswordAsync(string email, string password, string userName);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetUserByUserNameAsync(string username);
    }
    public class userRepo : EntityRepo<User>, IUserRepoExtra
    {
        public userRepo(AuthDemoDbContext _dbContext) : base(_dbContext)
        {
        }

        public bool emailExist(string email)
        {
            return dbContext.Users.Any(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailPasswordAsync(string email, string password, string userName)
        {
            var user = await dbContext.Users.Include(u => u.UsersRole).FirstOrDefaultAsync(u => u.Email == email && u.UserName == userName);

            if (user == null)
                return null;

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.HashPassword, password);

            if (result == PasswordVerificationResult.Success)
                return user;
            return null;
        }

        public async Task<User> GetUserByIdWithRolesAsync(int id)
        {
            return await dbContext.Users.Include(u => u.UsersRole).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }

        public bool userNameExist(string userName)
        {
            return dbContext.Users.Any(u => u.UserName == userName);
        }
    }
}
