using Microsoft.AspNetCore.Mvc;

namespace demopetshop.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", "Login");
        }

        [HttpGet]
        public IActionResult LoggedIn()
        {
            return View("LoggedIn", "Login");
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var storedUsername = _configuration["LoginCredentials:Username"];
            var storedPassword = _configuration["LoginCredentials:Password"];

            if (username == storedUsername && password == storedPassword)
            {
                // Authentication successful
                return RedirectToAction("LoggedIn", "Login");
            }
            else
            {
                // Authentication failed
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View("Index", "Login");
            }
        }
    }
}