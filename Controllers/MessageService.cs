using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace demobankapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class messageservice : ControllerBase
    {
        /// <summary>
        /// Send a support message.
        /// </summary>
        /// <response code="200">Message successfully sent.</response>
        // [Authorize]
        [HttpPost("send")]
        public IActionResult Send([FromBody] MessageModel messageSend)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new
            {
                FirstName = messageSend.FirstName,
                Email = messageSend.Email,
                Message = messageSend.Message
            });
        }

        public class MessageModel
        {
            [Required]
            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric")]
            public string FirstName { get; set; }

            [Required]
            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric")]
            public string LastName { get; set; }

            [Required]
            [RegularExpression(@"^\(\d{3}\) \d{3}-\d{4}$", ErrorMessage = "PhoneNumber format (###) ###-####")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            [Required]
            public string Message { get; set; }
        }
    }
}