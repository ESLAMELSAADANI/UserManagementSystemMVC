using Day06_Demo.Models;
using Day06_Demo.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Day06_Demo.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly EntityRepo<Role> roleRepo;
        private readonly IRole roleNameRepo;

        public RoleController(EntityRepo<Role> _roleRepo, IRole _roleNameRepo)
        {
            roleRepo = _roleRepo;
            roleNameRepo = _roleNameRepo;
        }

        public async Task<IActionResult> Index()
        {
            var model = await roleRepo.GetAllAsync();
            return View(model);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var model = await roleRepo.GetByIdAsync(id.Value);
            if (model == null)
                return NotFound();
            return View(model);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Role model)
        {
            if (ModelState.IsValid)
            {
                await roleRepo.AddAsync(model);
                await roleRepo.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View();
        }
        public IActionResult RoleExist([FromQuery(Name = "RoleName")] string roleName)
        {
            var roleExist = roleNameRepo.IsRoleExist(roleName);
            if (roleExist)
                return Json(false);
            else
                return Json(true);
        }
    }
}
