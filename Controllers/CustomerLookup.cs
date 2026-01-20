using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace demobankapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class customerlookup : ControllerBase
    {
        private Dictionary<string, CustDetails> CustDictionary = new Dictionary<string, CustDetails>
        {
            {"C895210", new CustDetails { CustID = "C895210", HeroName = "Iron Man", FirstName = "Anthony", LastName = "Stark", DOB = "1970/05/29", Address = "10880 Malibu Point, Malibu, California, 90265", PhoneNumber = "(123) 456-7890", Email = "tony.stark@starkindustries.com", SSN = "925-45-6789"}},
            {"C895211", new CustDetails { CustID = "C895211", HeroName = "Hawkeye", FirstName = "Clinton", LastName = "Barton", DOB = "1971/01/07", Address = "4525 W 38th Ave, New York City, NY, 10014", PhoneNumber = "(987) 654-3210", Email = "clint.barton@shield.gov", SSN = "912-56-7890"}},
            {"C895212", new CustDetails { CustID = "C895212", HeroName = "Black Widow", FirstName = "Natasha", LastName = "Romanoff", DOB = "1984/11/22", Address = "4525 W 38th Ave, New York City, NY, 10014", PhoneNumber = "(345) 678-9012", Email = "natasha.romanoff@shield.gov", SSN = "963-67-8901"}},
            {"C895213", new CustDetails { CustID = "C895213", HeroName = "War Machine", FirstName = "James", LastName = "Rhodes", DOB = "1968/10/06", Address = "1000 Defense Pentagon, Washington, DC 20301", PhoneNumber = "(456) 789-0123", Email = "james.rhodes@usaf.mil", SSN = "925-78-9012"}},
            {"C895214", new CustDetails { CustID = "C895214", HeroName = "Captain America", FirstName = "Steve", LastName = "Rogers", DOB = "1918/07/04", Address = "200 Park Avenue, New York City, NY, 10012", PhoneNumber = "(567) 890-1234", Email = "steve.rogers@avengers.com", SSN = "983-89-0123"}}
        };


        /// <summary>
        /// Get Customer information by Customer ID.
        /// </summary>
        /// <response code="200">Returns Customers information.</response>
        /// <response code="400">CustID field is missing or not accepted.</response>
        /// 
        [HttpGet("getbyid")]
        public IActionResult GetCustInfoByCustID([FromQuery] string CustID)
        {
            // Validate the CustID format using validation attributes
            var CustDetails = new CustDetails { CustID = CustID };

            var validationContext = new ValidationContext(CustDetails, serviceProvider: null, items: null)
            {
                MemberName = nameof(CustDetails.CustID)
            };

            var validationResults = new List<ValidationResult>();

            // Validate only the CustID property
            if (!Validator.TryValidateObject(CustDetails, validationContext, validationResults, validateAllProperties: true))
            {
                var validationErrors = validationResults.Select(result => result.ErrorMessage);
                return BadRequest(validationErrors);
            }

            // Find the matching customer using CustID as the key
            if (CustDictionary.TryGetValue(CustID, out var matchingCust))
            {
                var result = new
                {
                    CustID = matchingCust.CustID,
                    FirstName = matchingCust.FirstName,
                    LastName = matchingCust.LastName,
                    DOB = matchingCust.DOB,
                    Address = matchingCust.Address,
                    PhoneNumber = matchingCust.PhoneNumber,
                    Email = matchingCust.Email
                };

                return Ok(result);
            }

            var noCust = new
            {
                ErrorMessage = $"No Customer found with the ID '{CustID}'."
            };

            return NotFound(noCust);
        }


        /// <summary>
        /// Get Customer information by Last Name.
        /// </summary>
        /// <response code="200">Returns Customer information.</response>
        /// <response code="400">Last Name field is missing or not accepted.</response>
        /// <response code="404">Customer not found.</response>
        [HttpGet("getbylastname")]
        public IActionResult GetCustInfoByName([FromQuery] string lastname)
        {
            // Validate the LastName format using validation attributes
            var CustDetails = new CustDetails { LastName = lastname };

            var validationContext = new ValidationContext(CustDetails, serviceProvider: null, items: null)
            {
                MemberName = nameof(CustDetails.LastName)
            };

            var validationResults = new List<ValidationResult>();

            // Validate only the LastName property
            if (!Validator.TryValidateObject(CustDetails, validationContext, validationResults, validateAllProperties: true))
            {
                var validationErrors = validationResults.Select(result => result.ErrorMessage);
                return BadRequest(validationErrors);
            }

            // Search for customers with the specified LastName
            var matchingCusts = CustDictionary.Values
                .Where(c => string.Equals(c.LastName, lastname, StringComparison.OrdinalIgnoreCase))
                .Select(c => new
                {
                    CustID = c.CustID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DOB = c.DOB,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                })
                .ToList();

            if (matchingCusts.Any())
            {
                return Ok(matchingCusts);
            }

            var noCust = new
            {
                ErrorMessage = $"No Customers found with the last name '{lastname}'."
            };

            return NotFound(noCust);
        }

        /// <summary>
        /// Get Customer information by Hero Name.
        /// </summary>
        /// <response code="200">Returns Customer information.</response>
        /// <response code="400">Last Name field is missing or not accepted.</response>
        /// <response code="404">Customer not found.</response>
        [HttpGet("getbyheroname")]
        public IActionResult GetCustInfoByHeroName([FromQuery] string heroname)
        {
            // Validate the LastName format using validation attributes
            var CustDetails = new CustDetails { HeroName = heroname };

            var validationContext = new ValidationContext(CustDetails, serviceProvider: null, items: null)
            {
                MemberName = nameof(CustDetails.HeroName)
            };

            var validationResults = new List<ValidationResult>();

            // Validate only the LastName property
            if (!Validator.TryValidateObject(CustDetails, validationContext, validationResults, validateAllProperties: true))
            {
                var validationErrors = validationResults.Select(result => result.ErrorMessage);
                return BadRequest(validationErrors);
            }

            // Search for customers with the specified LastName
            var matchingCusts = CustDictionary.Values
                .Where(c => string.Equals(c.HeroName, heroname, StringComparison.OrdinalIgnoreCase))
                .Select(c => new
                {
                    CustID = c.CustID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DOB = c.DOB,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                })
                .ToList();

            if (matchingCusts.Any())
            {
                return Ok(matchingCusts);
            }

            var noCust = new
            {
                ErrorMessage = $"No Customers found with the hero name '{heroname}'."
            };

            return NotFound(noCust);
        }

        /// <summary>
        /// Get Customer information by Date of Birth.
        /// </summary>
        /// <response code="200">Returns Customer information.</response>
        /// <response code="400">DOB field is missing or not accepted.</response>
        /// <response code="404">Customer not found.</response>
        [HttpGet("getbydob")]
        public IActionResult GetCustInfoByDOB([FromQuery] string DOB)
        {
            // Validate the DOB format using validation attributes if necessary
            var CustDetails = new CustDetails { DOB = DOB };

            // Specify the member name to validate (DOB in this case)
            var validationContext = new ValidationContext(CustDetails, serviceProvider: null, items: null)
            {
                MemberName = nameof(CustDetails.DOB)
            };

            var validationResults = new List<ValidationResult>();

            // Validate only the DOB property
            if (!Validator.TryValidateObject(CustDetails, validationContext, validationResults, validateAllProperties: true))
            {
                // Validation failed for DOB
                var validationErrors = validationResults.Select(result => result.ErrorMessage);
                return BadRequest(validationErrors);
            }

            // Search for customers with the specified DOB
            var matchingCusts = CustDictionary.Values
                .Where(c => c.DOB == DOB)
                .Select(c => new
                {
                    CustID = c.CustID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DOB = c.DOB,
                    SSN = c.SSN,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                })
                .ToList();

            if (matchingCusts.Any())
            {
                return Ok(matchingCusts);
            }

            var noCust = new
            {
                ErrorMessage = $"No Customers found with the DOB '{DOB}'."
            };

            return NotFound(noCust);
        }

        /// <summary>
        /// Get information about all Customers.
        /// </summary>
        /// <response code="200">Returns Customer information.</response>
        /// <response code="404">No Customers are found.</response>
        [HttpGet("getallcustomers")]
        public IActionResult GetAllCust()
        {
            // Retrieve all customers from the dictionary
            var allCust = CustDictionary.Values
                .Select(c => new
                {
                    CustID = c.CustID,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DOB = c.DOB,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                })
                .ToList();

            if (allCust.Any())
            {
                return Ok(allCust);
            }

            var noCust = new
            {
                ErrorMessage = "No Customer found in the dictionary."
            };

            return NotFound(noCust);
        }

        public class CustDetails
        {
            [RegularExpression("^[a-zA-Z][0-9]{6}$", ErrorMessage = "CustID must start with a letter followed by a 6-digit number")]
            public string CustID { get; set; }

            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric")]
            public string HeroName { get; set; }

            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric")]
            public string FirstName { get; set; }

            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "LastName must be alphanumeric")]
            public string LastName { get; set; }

            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Address must be alphanumeric")]
            public string Address { get; set; }

            [RegularExpression("^\\(\\d{3}\\) \\d{3}-\\d{4}$", ErrorMessage = "PhoneNumber format (###) ###-####")]
            public string PhoneNumber { get; set; }

            [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Incorrect Email format")]
            public string Email { get; set; }

            [RegularExpression("^\\d{3}-\\d{2}-\\d{4}$", ErrorMessage = "Social Security Number format XXX-XX-XXXX")]
            public string SSN { get; set; }

            [RegularExpression("^(19|20)\\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\\d|3[01])$", ErrorMessage = "Date of Birth format YYYY/MM/DD")]
            public string DOB { get; set; }
        }
    }
}