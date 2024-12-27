using Microsoft.AspNetCore.Mvc;
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
        // Check if user is authenticated
        if (HttpContext.Session.GetString("IsAuthenticated") == "true")
        {
            return View("LoggedIn", "Login");
        }
        else
        {
            return RedirectToAction("Index", "Login");
        }
    }

    [HttpPost]
    public IActionResult Index(string username, string password)
    {
        var storedUsername = _configuration["LoginCredentials:Username"];
        var storedPassword = _configuration["LoginCredentials:Password"];

        if (username == storedUsername && password == storedPassword)
        {
            // Authentication successful, store in session
            HttpContext.Session.SetString("IsAuthenticated", "true");
            return RedirectToAction("LoggedIn", "Login");
        }
        else
        {
            // Authentication failed
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View("Index", "Login");
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        // Clear authentication session
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Login");
    }
}
