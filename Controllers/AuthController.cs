using Gym.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // التحقق من أن اليوزرنيم غير مستخدم بالفعل
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                ViewBag.Error = "This username is already taken. Please choose another one.";
                return View(user);
            }

            // حفظ المستخدم الجديد
            _context.Users.Add(user);
            _context.SaveChanges();

            // بعد التسجيل الناجح نوجهه لصفحة اللوجن
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // لو عايزة ممكن تحفظي اليوزر في session
            // HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult ChooseAuth()
        {
            return View();
        }


    }
}
