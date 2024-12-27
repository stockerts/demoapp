using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class BotController : Controller
{
    private readonly HttpClient _httpClient;

    public BotController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    // Serve the Bot page
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Handle POST requests from the Bot page
    [HttpPost]
    public async Task<IActionResult> PostToLogin([FromForm] string protocol, [FromForm] string domain, [FromForm] string port)
    {
        try
        {
            // Construct the dynamic URL
            var url = $"{protocol}://{domain}:{port}/login";

            // Prepare the POST request body
            var content = new StringContent("username=admin&password=admin", Encoding.UTF8, "application/x-www-form-urlencoded");

            // Send the POST request
            var response = await _httpClient.PostAsync(url, content);

            // Retrieve response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Return the response content as HTML
            return Content(responseContent, "text/html");
        }
        catch (Exception ex)
        {
            // Handle and display errors
            return Content($"<p style='color:red;'>Error: {ex.Message}</p>", "text/html");
        }
    }
}
