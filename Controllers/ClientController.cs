using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Client.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            // Get the client's IP address
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Remove the ::ffff: prefix if it's present (IPv4-mapped IPv6 address)
            if (!string.IsNullOrEmpty(ipAddress) && ipAddress.StartsWith("::ffff:"))
            {
                ipAddress = ipAddress.Substring(7); // Remove the "::ffff:" part
            }

            // Check if the IP is private or public
            var ipType = GetIpType(ipAddress);

            // Get the X-Forwarded-For header (if any)
            var xForwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].ToString();

            // Get the Host header (if any)
            var host = HttpContext.Request.Headers["Host"].ToString();

            // Get the User-Agent header
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            // Prepare the response object
            var model = new ClientInfoViewModel
            {
                IpAddress = string.IsNullOrEmpty(ipAddress)
                    ? "Unable to determine IP address."
                    : $"{ipAddress} ({ipType})",
                XForwardedFor = string.IsNullOrEmpty(xForwardedFor) ? "No X-Forwarded-For header present." : xForwardedFor,
                Host = string.IsNullOrEmpty(host) ? "No Host header present." : host,
                UserAgent = userAgent
            };

            return View(model);
        }

        public IActionResult Header()
        {
            // Create a list to store all header key-value pairs
            var headers = new List<HeaderInfo>();

            // Iterate over all headers in the request
            foreach (var header in HttpContext.Request.Headers)
            {
                headers.Add(new HeaderInfo
                {
                    Key = header.Key,
                    Value = header.Value.ToString()
                });
            }

            // Prepare the response object
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

            // Check if the IP is private
            if (IsPrivateIP(ip))
                return "Private";

            // Otherwise, it's considered public
            return "Public";
        }

        private bool IsPrivateIP(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();

            // Check for private IP ranges
            if (bytes[0] == 10) // 10.0.0.0 to 10.255.255.255
                return true;

            if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31) // 172.16.0.0 to 172.31.255.255
                return true;

            if (bytes[0] == 192 && bytes[1] == 168) // 192.168.0.0 to 192.168.255.255
                return true;

            return false;
        }
    }

    public class ClientInfoViewModel
    {
        public string IpAddress { get; set; }
        public string XForwardedFor { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
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
