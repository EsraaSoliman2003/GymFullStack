using Gym.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class ManageTrainersController : Controller
    {
        private readonly AppDbContext _context;

        public ManageTrainersController(AppDbContext context)
        {
            _context = context;
        }

        // عرض كل المدربين
        public IActionResult Index()
        {
            var trainers = _context.Users.Where(u => u.Role == "Trainer").ToList();
            return View(trainers);
        }

        // إنشاء مدرب جديد
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
                user.Role = "Trainer";
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // تعديل مدرب
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var trainer = _context.Users.Find(id);
            return View(trainer);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                user.Role = "Trainer";
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // حذف مدرب
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var trainer = _context.Users.Find(id);
            if (trainer != null)
            {
                _context.Users.Remove(trainer);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
