using Gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gym.Controllers
{
    public class ManageSubscriptionsController : Controller
    {
        private readonly AppDbContext _context;

        public ManageSubscriptionsController(AppDbContext context)
        {
            _context = context;
        }

        // مساعدة: تحميل قائمة المتدربين
        private void PopulateTraineesList(object selectedTrainee = null)
        {
            // جلب المتدربين (يجب أن يتم ربطهم بجدول Users للحصول على Username)
            var trainees = _context.Trainees
                .Include(t => t.User) // افترض أن هناك علاقة User
                .OrderBy(t => t.User.Username)
                .Select(t => new
                {
                    t.Id,
                    Username = t.User.Username + " (ID: " + t.Id + ")"
                });

            ViewBag.TraineeId = new SelectList(trainees, "Id", "Username", selectedTrainee);
        }

        // 1. القراءة (Index): عرض كل الاشتراكات النشطة
        public async Task<IActionResult> Index()
        {
            // تحميل الاشتراكات مع بيانات المتدرب المرتبطة
            var subscriptions = _context.Subscriptions
                .Include(s => s.Trainee)
                .ThenInclude(t => t.User); // لعرض اسم المتدرب

            return View(await subscriptions.ToListAsync());
        }

        // 2. الإنشاء (GET): عرض نموذج الإضافة
        [HttpGet]
        public IActionResult Create()
        {
            PopulateTraineesList();
            return View();
        }
        // في ملف ManageSubscriptionsController.cs

        // 💡 يجب استقبال اسم المستخدم كـ string بالإضافة إلى كائن الاشتراك
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subscription subscription, string TraineeUsername)
        {
            // أولاً نتأكد إن الداتا الاساسية مظبوطة
            if (!ModelState.IsValid)
            {
                return View(subscription);
            }

            // 1) نجيب اليوزر اللي Role = Trainee
            var traineeUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == TraineeUsername && u.Role == "Trainee");

            if (traineeUser == null)
            {
                ViewData["TraineeError"] = "The specified Username does not exist or is not a Trainee.";
                ViewBag.TraineeUsername = TraineeUsername;
                return View(subscription);
            }

            // 2) نجيب أو ننشئ الـ Trainee المرتبط باليوزر ده
            var traineeRecord = await _context.Trainees
                .FirstOrDefaultAsync(t => t.UserId == traineeUser.Id);

            if (traineeRecord == null)
            {
                traineeRecord = new Trainee
                {
                    UserId = traineeUser.Id,
                    Goal = "Fitness",     // قيمة افتراضية
                    DateOfBirth = null
                };

                _context.Trainees.Add(traineeRecord);
                await _context.SaveChangesAsync(); // عشان الـ Id يتولد
            }

            // 3) نتاكد إن مفيش اشتراك قديم لنفس المتدرب (عشان عندك Unique على TraineeId)
            var existingSub = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.TraineeId == traineeRecord.Id);

            if (existingSub != null)
            {
                ModelState.AddModelError(string.Empty, "This trainee already has a subscription.");
                ViewBag.TraineeUsername = TraineeUsername;
                return View(subscription);
            }

            // 4) نربط الاشتراك بالمتدرب
            subscription.TraineeId = traineeRecord.Id;

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // 3. التعديل (GET): جلب بيانات اشتراك للتعديل
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null) return NotFound();

            PopulateTraineesList(subscription.TraineeId);
            return View(subscription);
        }

        // 3. التعديل (POST): حفظ التعديلات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subscription subscription)
        {
            if (id != subscription.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateTraineesList(subscription.TraineeId);
            return View(subscription);
        }



        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1. العثور على الاشتراك في قاعدة البيانات
            var subscription = await _context.Subscriptions.FindAsync(id);

            // 2. التحقق والحذف
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);

                // 3. حفظ التغييرات في قاعدة البيانات
                await _context.SaveChangesAsync();
            }

            // 4. إعادة التوجيه إلى صفحة القائمة
            return RedirectToAction(nameof(Index));
        }
    }
}
