using Day06_Demo.DAL;
using Day06_Demo.Models;
using Day06_Demo.Repos;
using Microsoft.EntityFrameworkCore;

namespace Day06_Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(s =>
            {
                s.IdleTimeout = TimeSpan.FromMinutes(30);
                s.Cookie.Name = "MySessionCookie";
            });
            builder.Services.AddAuthentication("MyCookieAuth")
                            .AddCookie("MyCookieAuth", options =>
                            {
                                options.Cookie.Name = "MyCookieAuth";
                                options.LoginPath = "/Account/Login";
                                options.AccessDeniedPath = "/Account/AccessDenied";
                            });
            builder.Services.AddDbContext<AuthDemoDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDemoDb_Conn"));
            });
            builder.Services.AddScoped<EntityRepo<User>, userRepo>();
            builder.Services.AddScoped<EntityRepo<Role>, roleRepo>();
            builder.Services.AddScoped<EntityRepo<UserRole>, userRoleRepo>();
            builder.Services.AddScoped<IUserRoleRepo, userRoleRepo>();
            builder.Services.AddScoped<IUserRepoExtra, userRepo>();
            builder.Services.AddScoped<IRole, roleRepo>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
