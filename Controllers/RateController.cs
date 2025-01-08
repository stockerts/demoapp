using Microsoft.AspNetCore.Mvc;

public class RateController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RateResult(int reqNumber, int timeSec)
    {
        ViewData["reqNumber"] = reqNumber;
        ViewData["timeSec"] = timeSec;

        return View();
    }
}
