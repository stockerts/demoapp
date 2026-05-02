using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Client.Controllers
{
    public class ClientController : Controller
    {
        private readonly IConfiguration _configuration;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            // Get SERVER_NAME from environment variable only
            var serverName = _configuration["SERVER_NAME"];

            // Get the client's IP address
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Remove the ::ffff: prefix if present (IPv4-mapped IPv6)
            if (!string.IsNullOrEmpty(ipAddress) && ipAddress.StartsWith("::ffff:"))
            {
                ipAddress = ipAddress.Substring(7);
            }

            // Determine if IP is private or public
            var ipType = GetIpType(ipAddress);

            // Get headers
            var xForwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            var host = HttpContext.Request.Headers["Host"].ToString();
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            // Extract client IP from first value of X-Forwarded-For
            // Strip ports from each IP in X-Forwarded-For
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                xForwardedFor = string.Join(", ", xForwardedFor.Split(',').Select(ip => StripPort(ip.Trim())));
            }

            var clientIp = !string.IsNullOrEmpty(xForwardedFor)
                ? xForwardedFor.Split(',')[0].Trim()
                : null;

            // Prepare model
            var model = new ClientInfoViewModel
            {
                ClientIP = clientIp,
                IpAddress = string.IsNullOrEmpty(ipAddress)
                    ? "Unable to determine IP address."
                    : $"{ipAddress} ({ipType})",
                XForwardedFor = string.IsNullOrEmpty(xForwardedFor)
                    ? "No X-Forwarded-For header present."
                    : xForwardedFor,
                Host = string.IsNullOrEmpty(host)
                    ? "No Host header present."
                    : host,
                UserAgent = userAgent,
                ServerName = serverName
            };

            return View(model);
        }

        public IActionResult Header()
        {
            var headers = new List<HeaderInfo>();

            foreach (var header in HttpContext.Request.Headers)
            {
                headers.Add(new HeaderInfo
                {
                    Key = header.Key,
                    Value = header.Value.ToString()
                });
            }

            var model = new HeaderInfoViewModel
            {
                Headers = headers
            };

            return View(model);
        }

        private string GetIpType(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return "Unknown";

            var ip = IPAddress.Parse(ipAddress);

            if (IsPrivateIP(ip))
                return "Private";

            return "Public";
        }

        private string StripPort(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return ip;
            if (ip.StartsWith("["))
            {
                var bracket = ip.IndexOf(']');
                return bracket > 0 ? ip.Substring(1, bracket - 1) : ip;
            }
            var colonCount = ip.Count(c => c == ':');
            if (colonCount == 1)
                return ip.Substring(0, ip.LastIndexOf(':'));
            return ip;
        }

        private bool IsPrivateIP(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();

            if (bytes[0] == 10)
                return true;

            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
                return true;

            if (bytes[0] == 192 && bytes[1] == 168)
                return true;

            return false;
        }
    }

    public class ClientInfoViewModel
    {
        public string ClientIP { get; set; }
        public string IpAddress { get; set; }
        public string XForwardedFor { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string ServerName { get; set; }
    }

    public class HeaderInfoViewModel
    {
        public List<HeaderInfo> Headers { get; set; }
    }

    public class HeaderInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}