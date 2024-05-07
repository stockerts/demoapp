using Microsoft.AspNetCore.Mvc;

public class ResponseController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult NotFound()
    {
        Response.StatusCode = 404;
        return View("NotFound");
    }

    public IActionResult Forbidden()
    {
        Response.StatusCode = 403;
        return View("Forbidden");
    }

    public IActionResult Unavailable()
    {
        Response.StatusCode = 503;
        return View("Unavailable");
    }
}
