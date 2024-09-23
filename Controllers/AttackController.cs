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
    public IActionResult Bot()
    {
        return View("Bot");
    }
    public IActionResult Bot_Fail()
    {
        return View("Bot_Fail");
    }

}
