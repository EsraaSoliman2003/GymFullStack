using System.Diagnostics;
using Gym.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            ViewData["Title"] = "About Us";
            ViewBag.Message = "Learn more about our gym and what we offer!";
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact Us";
            ViewBag.Message = "We’d love to hear from you!";
            return View();
        }

        // Optional: Error Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
