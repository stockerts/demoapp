using Microsoft.AspNetCore.Mvc;

public class UserAgentController : Controller
{
    private readonly HttpClient _httpClient;

    public UserAgentController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendRequest([FromForm] string protocol, [FromForm] string domain, [FromForm] string port)
    {
        try
        {
            string url = $"{protocol}://{domain}:{port}/useragent";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.UserAgent.ParseAdd("badactor");
            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            return Content(body, "text/html");
        }
        catch (Exception ex)
        {
            return Content($"<p style='color:red;'>Error: {ex.Message}</p>", "text/html");
        }
    }
}
