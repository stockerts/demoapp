using Microsoft.AspNetCore.Mvc;
using System.Text;

public class ChallengeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChallengeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Called by RunHeader — checks if the proxy injected the challenge header
    [HttpGet]
    public IActionResult CheckHeader()
    {
        bool found = Request.Headers.TryGetValue("challenge", out var values) &&
                     values.ToString().Equals("test", StringComparison.OrdinalIgnoreCase);
        return Json(new { found });
    }

    [HttpPost]
    public async Task<IActionResult> RunHeader([FromForm] string protocol, [FromForm] string domain, [FromForm] string port)
    {
        try
        {
            string url = $"{protocol}://{domain}:{port}/Challenge/CheckHeader";
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            bool pass = body.Contains("\"found\":true", StringComparison.OrdinalIgnoreCase);
            return Json(new { pass, detail = pass ? "Header 'challenge: test' detected" : "Header 'challenge: test' not found" });
        }
        catch (Exception ex)
        {
            return Json(new { pass = false, detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RunRedirect([FromForm] string protocol, [FromForm] string domain, [FromForm] string port)
    {
        try
        {
            string url = $"{protocol}://{domain}:{port}/old/login";
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);

            bool pass = response.StatusCode == System.Net.HttpStatusCode.OK;
            string detail = pass
                ? $"Redirect resolved to 200 OK"
                : $"Received {(int)response.StatusCode} — no valid redirect";
            return Json(new { pass, detail });
        }
        catch (Exception ex)
        {
            return Json(new { pass = false, detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RunRateLimit([FromForm] string protocol, [FromForm] string domain, [FromForm] string port)
    {
        try
        {
            string url = $"{protocol}://{domain}:{port}/login";
            var client = _httpClientFactory.CreateClient();

            var tasks = Enumerable.Range(0, 10).Select(_ => client.GetAsync(url)).ToList();
            var responses = await Task.WhenAll(tasks);

            bool pass = false;
            foreach (var r in responses)
            {
                var body = await r.Content.ReadAsStringAsync();
                if (r.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                    r.StatusCode == System.Net.HttpStatusCode.TooManyRequests ||
                    body.Contains("blocked", StringComparison.OrdinalIgnoreCase) ||
                    body.Contains("rejected", StringComparison.OrdinalIgnoreCase))
                {
                    pass = true;
                    break;
                }
            }

            string detail = pass ? "Rate limit enforced — request blocked" : "All 10 requests succeeded — not rate limited";
            return Json(new { pass, detail });
        }
        catch (Exception ex)
        {
            return Json(new { pass = false, detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RunWAF([FromForm] string protocol, [FromForm] string domain, [FromForm] string port)
    {
        try
        {
            string url = $"{protocol}://{domain}:{port}/login?id=1%27+OR+%271%27%3D%271";
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            bool pass = response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                        body.Contains("blocked", StringComparison.OrdinalIgnoreCase) ||
                        body.Contains("rejected", StringComparison.OrdinalIgnoreCase);

            string detail = pass ? "SQL injection blocked by WAF" : "SQL injection not blocked — WAF not active";
            return Json(new { pass, detail });
        }
        catch (Exception ex)
        {
            return Json(new { pass = false, detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RunBotDeclared([FromForm] string protocol, [FromForm] string domain, [FromForm] string port)
    {
        try
        {
            string url = $"{protocol}://{domain}:{port}/login";
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent("username=admin&password=admin", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")
            };
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; Ask Jeeves/Teoma;)");
            request.Headers.Add("DemoApp", "Bot");
            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            bool pass = response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                        body.Contains("blocked", StringComparison.OrdinalIgnoreCase) ||
                        body.Contains("rejected", StringComparison.OrdinalIgnoreCase);

            string detail = pass ? "Declared bot blocked" : "Declared bot not blocked — bot defense not active";
            return Json(new { pass, detail });
        }
        catch (Exception ex)
        {
            return Json(new { pass = false, detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> RunBotMasked([FromForm] string protocol, [FromForm] string domain, [FromForm] string port, [FromForm] string userAgent)
    {
        try
        {
            string url = $"{protocol}://{domain}:{port}/login";
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent("username=admin&password=admin", System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")
            };
            if (!string.IsNullOrWhiteSpace(userAgent))
                request.Headers.UserAgent.ParseAdd(userAgent);
            request.Headers.Add("DemoApp", "Bot");
            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            bool pass = response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                        body.Contains("blocked", StringComparison.OrdinalIgnoreCase) ||
                        body.Contains("rejected", StringComparison.OrdinalIgnoreCase);

            string detail = pass ? "Masked bot blocked" : "Masked bot not blocked — signal-based defense not active";
            return Json(new { pass, detail });
        }
        catch (Exception ex)
        {
            return Json(new { pass = false, detail = ex.Message });
        }
    }
}
