using Microsoft.AspNetCore.Mvc;

public class CatController : Controller
{
    private readonly CatService _catService;

    public CatController(CatService catService)
    {
        _catService = catService;
    }

    public IActionResult Index()
    {
        return View("Index");
    }
    public async Task<IActionResult> AllCats()
    {
        try
        {
            var cats = await _catService.GetAllCats();

            if (cats == null)
            {
                // Handle the case where cats is null
                return View("Error");
            }

            return View(cats.ToArray());
        }
        catch (HttpRequestException ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error calling API: {ex.Message}");

            // Handle the timeout or other HTTP request exceptions
            return View("Error");
        }
        catch (Exception ex)
        {
            // Handle other types of exceptions
            Console.WriteLine($"Error: {ex.Message}");
            return View("Error");
        }
    }
}
