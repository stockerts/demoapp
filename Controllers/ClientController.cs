using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            // Get the client's IP address
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Get the X-Forwarded-For header (if any)
            var xForwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].ToString();

            // Get the Host header (if any)
            var host = HttpContext.Request.Headers["Host"].ToString();

            // Get the User-Agent header
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            // Get the Server header (if any)
            var server = HttpContext.Request.Headers["Server"].ToString();

            // Prepare the response object
            var model = new ClientInfoViewModel
            {
                IpAddress = string.IsNullOrEmpty(ipAddress) ? "Unable to determine IP address." : ipAddress,
                XForwardedFor = string.IsNullOrEmpty(xForwardedFor) ? "No X-Forwarded-For header present." : xForwardedFor,
                Host = string.IsNullOrEmpty(host) ? "No Host header present." : host,
                UserAgent = userAgent,
                Server = string.IsNullOrEmpty(server) ? "No Server header present." : server
            };

            return View(model);
        }
    }

    public class ClientInfoViewModel
    {
        public string IpAddress { get; set; }
        public string XForwardedFor { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string Server { get; set; }
    }
}
