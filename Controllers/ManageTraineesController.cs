using Gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (trainee == null)
            {
                return NotFound();
            }
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

            // 1. البحث عن المستخدم (User)
            var userToDelete = _context.Users.Find(id);

            if (userToDelete != null && userToDelete.Role == "Trainee")
            {
                // 2. البحث عن سجل المتدرب المرتبط (Trainee)
                // يجب استخدام Include() هنا لضمان تحميل جميع السجلات المرتبطة إذا لم تكن العلاقة محملة
                var traineeRecord = _context.Trainees
                    .Include(t => t.WorkoutPlans) // تحميل خطط التمارين
                    .Include(t => t.DietPlans)     // تحميل خطط الحمية
                    .Include(t => t.Subscription) // تحميل الاشتراكات
                    .FirstOrDefault(t => t.UserId == id);

                if (traineeRecord != null)
                {
                    // 3. الحذف المتتالي (حذف السجلات التابعة أولاً)

                    // حذف خطط التمارين (WorkoutPlans)
                    if (traineeRecord.WorkoutPlans != null)
                    {
                        _context.WorkoutPlans.RemoveRange(traineeRecord.WorkoutPlans);
                    }

                    // حذف خطط الحمية (DietPlans)
                    if (traineeRecord.DietPlans != null)
                    {
                        _context.DietPlans.RemoveRange(traineeRecord.DietPlans);
                    }

                    // حذف الاشتراك (Subscriptions)
                    // (بما أن العلاقة One-to-One/Zero-or-One، يجب التعامل معها كسجل واحد)
                    if (traineeRecord.Subscription != null)
                    {
                        _context.Subscriptions.Remove(traineeRecord.Subscription);
                    }

                    // 4. حذف سجل المتدرب نفسه من جدول Trainees
                    _context.Trainees.Remove(traineeRecord);
                }

                // 5. حذف سجل المستخدم من جدول Users
                _context.Users.Remove(userToDelete);

                // 6. حفظ التغييرات
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        //var trainee = _context.Users.Find(id);
        //if (trainee != null && trainee.Role == "Trainee")
        //{
        //    _context.Users.Remove(trainee);
        //    _context.SaveChanges();
        //}
        //return RedirectToAction("Index");
    }
    
}
