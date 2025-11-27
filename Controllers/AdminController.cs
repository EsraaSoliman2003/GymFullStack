using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Gym.Models;

namespace Gym.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            base.OnActionExecuting(context);
        }

        // Main Dashboard
        public IActionResult Index()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalTrainers = _context.Users.Count(u => u.Role == "Trainer");
            ViewBag.TotalTrainees = _context.Users.Count(u => u.Role == "Trainee");
            TempData["AdminWelcome"] = "You are logged in as Admin";

            return View();
        }

        // Reports Page






        public IActionResult Reports()
        {
            // احصلي الأعداد لكل Role
            ViewBag.TotalUsers = _context.Users.Count(); // كل اليوزرز
            ViewBag.TotalTrainers = _context.Users.Count(u => u.Role == "Trainer");
            ViewBag.TotalTrainees = _context.Users.Count(u => u.Role == "Trainee");



            return View();
        }

        // في ملف AdminController.cs

        public IActionResult ManageTrainers()
        {
            // هذا الإجراء يقوم بتوجيه المسؤول مباشرة إلى الإندكس الخاص بوحدة تحكم إدارة المدربين
            // هذا يضمن أن يتم عرض قائمة المدربين
            return RedirectToAction("Index", "ManageTrainers");
        }




    }
}
