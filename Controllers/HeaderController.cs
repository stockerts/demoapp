using Microsoft.AspNetCore.Mvc;

namespace Header.Controllers
{
    public class HeaderController : Controller
    {
        public IActionResult Index()
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