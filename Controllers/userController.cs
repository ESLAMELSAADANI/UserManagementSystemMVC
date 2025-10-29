using Day06_Demo.Models;
using Day06_Demo.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Day06_Demo.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [AllowAnonymous]
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
        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            if (ModelState.IsValid)
            {
                userRepo.Update(model);
                await userRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View();
        }
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
        public IActionResult Add()
        {
            return View();
        }
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
        public IActionResult EmailExist([FromQuery(Name = "Email")] string email)
        {
            bool emailExist = userExtraRepo.emailExist(email);
            if (emailExist)
                return Json(false);
            else
                return Json(true);
        }
        [AllowAnonymous]
        public IActionResult UserNameExist([FromQuery(Name = "UserName")] string userName)
        {
            bool userNameExist = userExtraRepo.userNameExist(userName);
            if (userNameExist)
                return Json(false);
            else
                return Json(true);
        }

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
