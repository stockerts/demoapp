using Microsoft.AspNetCore.Mvc;

public class AttackController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SQL()
    {
        return View("SQL");
    }

    public IActionResult DIR()
    {
        return View("DIR");
    }
    public IActionResult XSS()
    {
        return View("XSS");
    }
}
