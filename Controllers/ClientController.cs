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
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress))
            {
                return NotFound("Unable to determine client IP address.");
            }

            return Ok(new { IpAddress = ipAddress });
        }
    }
}