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

        public async Task<IActionResult> Dashboard(string? search = null)
        {
            var traineesQuery = _context.Trainees
                .Include(t => t.User)
                .Include(t => t.WorkoutPlans)
                .Include(t => t.DietPlans)
                .Include(t => t.Subscription)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                traineesQuery = traineesQuery.Where(t =>
                    t.User != null && t.User.Username.Contains(search));
            }

            var trainees = await traineesQuery.ToListAsync();

            ViewBag.TotalTrainees = trainees.Count;
            ViewBag.TotalWorkoutPlans = trainees.Sum(t => t.WorkoutPlans?.Count ?? 0);
            ViewBag.TotalDietPlans = trainees.Sum(t => t.DietPlans?.Count ?? 0);

            return View(trainees);
        }

        public async Task<IActionResult> TraineeDetails(int id)
        {
            var trainee = await _context.Trainees
                .Include(t => t.User)
                .Include(t => t.WorkoutPlans)
                .Include(t => t.DietPlans)
                .Include(t => t.Subscription)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainee == null)
                return NotFound();

            return View(trainee);
        }


        public async Task<IActionResult> ManageWorkout(int traineeId)
        {
            var trainee = await _context.Trainees
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == traineeId);

            if (trainee == null)
                return NotFound();

            var plans = await _context.WorkoutPlans
                .Where(p => p.TraineeId == traineeId)
                .ToListAsync();

            ViewBag.Trainee = trainee;
            return View(plans);
        }

[HttpGet]
public IActionResult AddWorkout(int traineeId)
{
    if (traineeId <= 0)
    {
        // لو حد فتح اللينك من غير TraineeId
        return BadRequest("TraineeId is required");
    }

    var model = new WorkoutPlan
    {
        TraineeId = traineeId
    };

    return View(model);
}


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddWorkout(WorkoutPlan model)
{
    if (!ModelState.IsValid)
    {
        return View(model);
    }

    _context.WorkoutPlans.Add(model);
    await _context.SaveChangesAsync();

    return RedirectToAction("ManageWorkout", new { traineeId = model.TraineeId });
}


        [HttpGet]
        public async Task<IActionResult> EditWorkout(int id)
        {
            var plan = await _context.WorkoutPlans.FindAsync(id);
            if (plan == null)
                return NotFound();

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWorkout(int id, WorkoutPlan model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            _context.WorkoutPlans.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageWorkout), new { traineeId = model.TraineeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var plan = await _context.WorkoutPlans.FindAsync(id);
            if (plan != null)
            {
                var traineeId = plan.TraineeId;
                _context.WorkoutPlans.Remove(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageWorkout), new { traineeId });
            }

            return RedirectToAction(nameof(Dashboard));
        }


        public async Task<IActionResult> ManageDiet(int traineeId)
        {
            var trainee = await _context.Trainees
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == traineeId);

            if (trainee == null)
                return NotFound();

            var diets = await _context.DietPlans
                .Where(d => d.TraineeId == traineeId)
                .ToListAsync();

            ViewBag.Trainee = trainee;
            return View(diets);
        }

[HttpGet]
public IActionResult AddDiet(int traineeId)
{
    if (traineeId <= 0)
        return BadRequest("TraineeId is required");

    var model = new DietPlan
    {
        TraineeId = traineeId
    };
    return View(model);
}


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddDiet(DietPlan model)
{
    if (!ModelState.IsValid)
        return View(model);

    _context.DietPlans.Add(model);
    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(ManageDiet), new { traineeId = model.TraineeId });
}


        [HttpGet]
        public async Task<IActionResult> EditDiet(int id)
        {
            var diet = await _context.DietPlans.FindAsync(id);
            if (diet == null)
                return NotFound();

            return View(diet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDiet(int id, DietPlan model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            _context.DietPlans.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageDiet), new { traineeId = model.TraineeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDiet(int id)
        {
            var diet = await _context.DietPlans.FindAsync(id);
            if (diet != null)
            {
                var traineeId = diet.TraineeId;
                _context.DietPlans.Remove(diet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageDiet), new { traineeId });
            }

            return RedirectToAction(nameof(Dashboard));
        }
    }
}
