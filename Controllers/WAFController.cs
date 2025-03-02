using Microsoft.AspNetCore.Mvc;

public class WAFController : Controller
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
