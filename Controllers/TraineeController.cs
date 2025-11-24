using Gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;

namespace Gym.Controllers
{
  
    public class TraineeController : Controller
    {

        private readonly AppDbContext _context;

        public TraineeController(AppDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Trainee")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            base.OnActionExecuting(context);
        }

        // Dashboard

        public async Task<IActionResult> Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var trainee = await _context.Trainees
                .Include(t => t.WorkoutPlans)
                .Include(t => t.DietPlans)
                .Include(t => t.Subscription)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainee == null)
                return NotFound();

            return View(trainee);
        }

        // Workout Plan
        public async Task<IActionResult> WorkoutPlan(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            
            var trainee = await _context.Trainees
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

            if (trainee == null) return Forbid(); 

            var plans = await _context.WorkoutPlans
                .Where(p => p.TraineeId == id)
                .ToListAsync();

            return View(plans);
        } 


        
        // Diet Plan
      
        public async Task<IActionResult> DietPlan(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var trainee = await _context.Trainees
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId.Value);

            if (trainee == null) return Forbid();

            var diets = await _context.DietPlans.Where(d => d.TraineeId == id).ToListAsync();
            return View(diets);
        }

        // Subscription
        [HttpGet]
        
        public async Task<IActionResult> Subscription(int id)
        {
            var sub = await _context.Subscriptions
                .Include(s => s.Trainee)
                .FirstOrDefaultAsync(s => s.TraineeId == id);

            if (sub == null)
               return NotFoundView();
          

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");
            if (sub.Trainee?.UserId != userId.Value) return Forbid();

            return View(sub);
        }

        // =============================
        [HttpGet] 
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

           
            var trainee = await _context.Trainees
                                .Include(t => t.User) 
                                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainee == null)
                return NotFound();

            return View(trainee);
        }

    
        [HttpPost] 
        [ValidateAntiForgeryToken] 
       
        public async Task<IActionResult> EditProfile([Bind("Goal,DateOfBirth")] Trainee formData)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            // هنجيب البيانات الأصلية للمتدرب من الداتابيز
            var traineeToUpdate = await _context.Trainees.FirstOrDefaultAsync(t => t.UserId == userId);

            if (traineeToUpdate == null)
                return NotFound();

            // بنعمل Validation (نتأكد إن الهدف مش فاضي)
            if (string.IsNullOrEmpty(formData.Goal))
            {
                ModelState.AddModelError("Goal", "Please select your goal.");
            }

            // لو كل البيانات سليمة
            if (ModelState.IsValid)
            {
                // بنحدّث البيانات الأصلية بالبيانات الجديدة اللي جاية من الفورم
                traineeToUpdate.Goal = formData.Goal;
                traineeToUpdate.DateOfBirth = formData.DateOfBirth;

                try
                {
                    _context.Update(traineeToUpdate); // بنقول للـ context إن الموديل ده اتعدل
                    await _context.SaveChangesAsync(); // بنحفظ التعديلات في الداتابيز
                }
                catch (DbUpdateException)
                {
                    // لو حصل أي مشكلة وقت الحفظ (نادرة)
                    ModelState.AddModelError("", "Unable to save changes. Try again.");
                    // هنرجع تاني لنفس الصفحة ونعرض الـ Error
                    // لازم نرجع بيانات اليوزر تاني لأن الفورم مش بيبعتها
                    traineeToUpdate.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == traineeToUpdate.UserId);
                    return View(traineeToUpdate);
                }

                // لو نجح الحفظ، رجعه للداشبورد
                return RedirectToAction(nameof(Dashboard));
            }

            // لو الموديل مش Valid (مثلاً ساب خانة "Goal" فاضية)
            // هنرجع لنفس الصفحة تاني، وهنعرضله الـ Error
            // لازم نرجع بيانات اليوزر تاني
            traineeToUpdate.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == traineeToUpdate.UserId);
            // بنرجع البيانات اللي هو دخلها الغلط عشان يشوفها
            traineeToUpdate.Goal = formData.Goal;
            traineeToUpdate.DateOfBirth = formData.DateOfBirth;

            return View(traineeToUpdate);
        }

        [NonAction]
        private IActionResult NotFoundView()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }




        public IActionResult Index()
        {
            return View();
        }


    }
}


