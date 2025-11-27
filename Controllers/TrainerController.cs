using Gym.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Gym.Controllers
{
    public class TrainerController : Controller
    {
        private readonly AppDbContext _context;

        public TrainerController(AppDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || role != "Trainer")
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            base.OnActionExecuting(context);
        }

        // Dashboard للـ Trainer
        public async Task<IActionResult> Dashboard()
        {
            // دلوقتي لسه مفيش relation بين المدرب والمتدربين
            // فهنعرض كل المتدربين، لكن مرتبطة بخططهم واشتراكهم
            var trainees = await _context.Trainees
                .Include(t => t.User)
                .Include(t => t.WorkoutPlans)
                .Include(t => t.DietPlans)
                .Include(t => t.Subscription)
                .ToListAsync();

            return View(trainees);
        }
    }
}
