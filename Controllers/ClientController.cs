using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetClientIp()
        {
            // Get the client's IP address
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Get the X-Forwarded-For header (if any)
            var xForwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].ToString();

            // Get the User-Agent header
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            // Get the Server header (if any)
            var server = HttpContext.Request.Headers["Server"].ToString();

            // Prepare the response object
            var response = new
            {
                IpAddress = string.IsNullOrEmpty(ipAddress) ? "Unable to determine IP address." : ipAddress,
                XForwardedFor = string.IsNullOrEmpty(xForwardedFor) ? "No X-Forwarded-For header present." : xForwardedFor,
                UserAgent = userAgent,
                ServerHeader = string.IsNullOrEmpty(server) ? "No Server header present." : server
            };

            return Ok(response);
        }
    }
}