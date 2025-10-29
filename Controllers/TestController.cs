using Microsoft.AspNetCore.Mvc;

namespace Day06_Demo.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WriteCookie(int id, string name)
        {
            Response.Cookies.Append("id", id.ToString(), new CookieOptions() { Expires = DateTime.Now.AddMinutes(3) });
            Response.Cookies.Append("fName", name);
            return View();
        }
        public IActionResult ReadCookie()
        {
            string id = Request.Cookies["id"];
            string name = Request.Cookies["fName"];

            ViewBag.id = id;
            ViewBag.name = name;
            return View();
        }
        public IActionResult WriteSession(int id, string name)
        {
            HttpContext.Session.SetInt32("id", id);
            HttpContext.Session.SetString("fName", name);
            return View();
        }
        public IActionResult ReadSession()
        {
            int? id = HttpContext.Session.GetInt32("id");
            string? name = HttpContext.Session.GetString("fName");

            ViewBag.id = id;
            ViewBag.name = name;
            return View();
        }
    }
}
