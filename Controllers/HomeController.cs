using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Info()
    {
        return View("Info");
    }

    // Action method to handle the 404 error
    public IActionResult NotFound()
    {
        return View("NotFound");
    }
}