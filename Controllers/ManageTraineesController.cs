using Gym.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class ManageTraineesController : Controller
    {
        private readonly AppDbContext _context;

        public ManageTraineesController(AppDbContext context)
        {
            _context = context;
        }

        // قائمة كل المتدربين
        public IActionResult Index()
        {
            var trainees = _context.Users.Where(u => u.Role == "Trainee").ToList();
            return View(trainees);
        }

        // إنشاء متدرب جديد
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "Trainee";
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // تعديل متدرب
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var trainee = _context.Users.Find(id);
            return View(trainee);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "Trainee";
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // حذف متدرب
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var trainee = _context.Users.Find(id);
            if (trainee != null)
            {
                _context.Users.Remove(trainee);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
