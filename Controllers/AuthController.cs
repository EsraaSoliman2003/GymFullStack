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

            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
            {
                ViewBag.Error = "This username is already taken. Please choose another one.";
                return View(user);
            }


            _context.Users.Add(user);
            _context.SaveChanges();


            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(string username, string password)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        //    if (user != null)
        //    {
        //        HttpContext.Session.SetString("Username", user.Username);
        //        HttpContext.Session.SetString("Role", user.Role);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ViewBag.Error = "Invalid username or password";
        //    return View();
        //}


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetInt32("UserId", user.Id);

                // بعد كده نحاول نجيب الـ Trainee اللي تابع للمستخدم ده
                if (user.Role == "Trainee")
                {
                    var trainee = _context.Trainees.FirstOrDefault(t => t.UserId == user.Id);

                    if (trainee == null)
                    {
                        trainee = new Trainee
                        {
                            UserId = user.Id,
                            Goal = "Fitness",
                            DateOfBirth = null
                        };

                        _context.Trainees.Add(trainee);
                        _context.SaveChanges();
                    }

                    return RedirectToAction("Dashboard", "Trainee", new { id = trainee.Id });
                }

                else if (user.Role == "Trainer")
                {
                    return RedirectToAction("Dashboard", "Trainer");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }



        public IActionResult ChooseAuth()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.EmailOrPhone))
            {
                ModelState.AddModelError("", "Please enter your email or phone number.");
                return View(model);
            }

            ViewBag.Message = "A reset link has been sent if the account exists.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginForAdmin(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Role == "Trainer")
                {
                    return RedirectToAction("Dashboard", "Trainer");
                }
                else // Trainee
                {
                    return RedirectToAction("Dashboard", "Trainee");
                }
            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }



    }
}
