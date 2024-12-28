using Microsoft.AspNetCore.Mvc;
using System.Text;

public class BotController : Controller
{
    private readonly HttpClient _httpClient;

    public BotController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    private string ConstructUrl(string protocol, string domain, string port, bool useTestUrl = false)
    {
        // Feature flag to enable test URL
        if (useTestUrl)
        {
            return "http://demotest:80";  // Use the test URL when the feature flag is true
        }
        return $"{protocol}://{domain}:{port}/login";  // Construct the URL dynamically
    }

    // Serve the Bot page
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Handle POST requests from the Bot page
    [HttpPost]
    public async Task<IActionResult> PostToLogin([FromForm] string protocol, [FromForm] string domain, [FromForm] string port, [FromForm] string userAgent)
    {
        try
        {
            // Construct the dynamic URL or use the test URL based on the feature flag
            string url = ConstructUrl(protocol, domain, port, useTestUrl: false);  // Set feature flag to false or true as needed

            // Prepare the POST request body
            var content = new StringContent("username=admin&password=admin", Encoding.UTF8, "application/x-www-form-urlencoded");

            // Set up the request message
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Add the User-Agent header
            request.Headers.UserAgent.ParseAdd(userAgent);
            
            // Add the DemoApp header
            request.Headers.Add("DemoApp", "Bot");

            // Send the POST request
            var response = await _httpClient.SendAsync(request);

            // Retrieve response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Check if the response contains "Invalid username or password."
            if (responseContent.Contains("Invalid username or password.", StringComparison.OrdinalIgnoreCase))
            {
                var errorMessage = $@"
                <p style='color:red;'>Error: Invalid username or password.</p>
                <p>Attempted URL: <strong>{url}</strong></p>";
                return Content(errorMessage, "text/html");
            }

            // Return the response content as HTML
            return Content(responseContent, "text/html");
        }
        catch (Exception ex)
        {
            // Handle and display errors, including the attempted URL
            var errorMessage = $@"
            <p style='color:red;'>Error: {ex.Message}</p>
            <p>Attempted URL: <strong>{ConstructUrl(protocol, domain, port)}</strong></p>";
            return Content(errorMessage, "text/html");
        }
    }
}
