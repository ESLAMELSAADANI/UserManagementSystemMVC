using Day06_Demo.Models;
using Day06_Demo.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Threading.Tasks;

namespace Day06_Demo.Controllers
{
    [Authorize]
    public class userController : Controller
    {
        public readonly EntityRepo<User> userRepo;
        public readonly IUserRoleRepo userRoleRepo;
        public readonly IUserRepoExtra userExtraRepo;

        public readonly IRole roleRepo;


        public userController(EntityRepo<User> _userRepo, IUserRepoExtra _userExtraRepo, IRole _roleRepo, IUserRoleRepo _userRoleRepo)
        {
            userRepo = _userRepo;
            userExtraRepo = _userExtraRepo;
            roleRepo = _roleRepo;
            userRoleRepo = _userRoleRepo;
        }

        public async Task<IActionResult> Index()
        {
            var model = await userRepo.GetAllAsync();
            return View(model);
        }
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = await userRepo.GetByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var user = await userExtraRepo.GetUserByIdWithRolesAsync(id.Value);
            if (user == null)
                return NotFound();

            var userRoles = await roleRepo.GetUserRolesAsync(user.Id);
            var model = new UserRoleVM()
            {
                User = user,
                RolesToDelete = userRoles
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            if (ModelState.IsValid)
            {
                userRepo.Update(model);
                await userRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            var user = await userRepo.GetByIdAsync(model.Id);
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var model = await userRepo.GetByIdAsync(id.Value);
            if (model == null)
                return NotFound();
            userRepo.Delete(model);
            await userRepo.SaveChangesAsync();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(User model)
        {
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<User>();

                model.HashPassword = hasher.HashPassword(model, model.HashPassword);

                await userRepo.AddAsync(model);
                await userRepo.SaveChangesAsync();
                Role role = await roleRepo.GetByNameAsync("user".ToLower());
                await userRoleRepo.AddAsync(model.Id, role.Id);
                await userRoleRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewRoles(int? id)
        {
            if (id == null)
                return BadRequest();
            var user = await userExtraRepo.GetUserByIdWithRolesAsync(id.Value);
            if (user == null)
                return NotFound();

            List<Role> rolesToDelete = await roleRepo.GetUserRolesAsync(id.Value);
            List<Role> rolesToAdd = await roleRepo.GetUserAnotherRolesAsync(id.Value);
            UserRoleVM userRoleVM = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            return View(userRoleVM);
        }
        [AllowAnonymous]
        public async Task<IActionResult> EmailExist([FromQuery(Name = "Email")] string email, [FromQuery(Name = "Id")] int id)
        {
            // When adding a new user (Id == 0)
            if (id == 0)
            {
                bool emailExist = userExtraRepo.emailExist(email);
                return Json(!emailExist); // true if email doesn't exist
            }

            // When editing an existing user
            var existingUser = await userExtraRepo.GetUserByEmailAsync(email);

            if (existingUser == null)
                return Json(true); // Email doesn't exist in DB → valid

            // Email exists but belongs to the same user being edited → valid
            if (existingUser.Id == id)
                return Json(true);

            // Email exists and belongs to another user → invalid
            return Json(false);
        }
        [AllowAnonymous]
        public async Task<IActionResult> UserNameExist([FromQuery(Name = "UserName")] string userName, [FromQuery(Name = "Id")] int id)
        {
            // When adding a new user (Id == 0)
            if (id == 0)
            {
                bool nameExist = userExtraRepo.userNameExist(userName);
                return Json(!nameExist); // true if username doesn't exist
            }

            // When editing an existing user
            var existingUser = await userExtraRepo.GetUserByUserNameAsync(userName);

            if (existingUser == null)
                return Json(true); // username doesn't exist in DB → valid

            // username exists but belongs to the same user being edited → valid
            if (existingUser.Id == id)
                return Json(true);

            // username exists and belongs to another user → invalid
            return Json(false);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserRole(int roleId, int userID)
        {
            UserRole userRole = await userRoleRepo.GetAsync(userID, roleId);
            userRoleRepo.Delete(userRole);
            await userRoleRepo.SaveChangesAsync();

            var user = await userRepo.GetByIdAsync(userID);
            var rolesToDelete = await roleRepo.GetUserRolesAsync(userID);
            var rolesToAdd = await roleRepo.GetUserAnotherRolesAsync(userID);

            var model = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            //await HttpContext.SignOutAsync("MyCookieAuth");
            //return RedirectToAction("Login","account");
            return View("ViewRoles", model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserRole(int roleId, int userID)
        {
            UserRole userRole = new UserRole() { UserId = userID, RoleId = roleId };
            await userRoleRepo.AddAsync(userRole);
            await userRoleRepo.SaveChangesAsync();

            var user = await userRepo.GetByIdAsync(userID);
            var rolesToDelete = await roleRepo.GetUserRolesAsync(userID);
            var rolesToAdd = await roleRepo.GetUserAnotherRolesAsync(userID);

            var model = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            //await HttpContext.SignOutAsync("MyCookieAuth");
            //return RedirectToAction("Login","account");
            return View("ViewRoles", model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSelectedRoles(UserRoleVM model)
        {
            if (model.RolesToDeleteIds != null && model.RolesToDeleteIds.Count > 0)
            {
                foreach (var roleId in model.RolesToDeleteIds)
                {
                    var userRole = await userRoleRepo.GetAsync(model.User.Id, roleId);
                    userRoleRepo.Delete(userRole);
                }

                await userRoleRepo.SaveChangesAsync();
                int userId = model.User.Id;
                return RedirectToAction("ViewRoles", new { id = userId });
            }
            ModelState.AddModelError("", "Select Roles To Delete FromThis User!");
            var user = model.User;
            var rolesToDelete = await roleRepo.GetUserRolesAsync(user.Id);
            var rolesToAdd = await roleRepo.GetUserAnotherRolesAsync(user.Id);

            UserRoleVM modell = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            return View("ViewRoles", modell);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSelectedRoles(UserRoleVM model)
        {
            if (model.RolesToAddIds != null && model.RolesToAddIds.Count > 0)
            {
                foreach (var roleId in model.RolesToAddIds)
                {
                    var userRole = new UserRole() { UserId = model.User.Id, RoleId = roleId };
                    await userRoleRepo.AddAsync(userRole);
                }

                await userRoleRepo.SaveChangesAsync();
                int userId = model.User.Id;
                return RedirectToAction("ViewRoles", new { id = userId });
            }
            ModelState.AddModelError("", "Select Roles To Add FromThis User!");
            var user = model.User;
            var rolesToDelete = await roleRepo.GetUserRolesAsync(user.Id);
            var rolesToAdd = await roleRepo.GetUserAnotherRolesAsync(user.Id);

            UserRoleVM modell = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete,
                RolesToAdd = rolesToAdd
            };
            return View("ViewRoles", modell);

        }
    }
}
