using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace demobankapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class accountlookup : ControllerBase
    {
        private Dictionary<string, CustDetails> CustDictionary = new Dictionary<string, CustDetails>
        {
            {"C895210", new CustDetails { CustID = "C895210", Savings = "$185,225,234,982", Checking = "$3,000"}},
            {"C895211", new CustDetails { CustID = "C895211", Savings = "$1,648", Checking = "$5,201"}},
            {"C895212", new CustDetails { CustID = "C895212", Savings = "$368,256", Checking = "$865"}},
            {"C895213", new CustDetails { CustID = "C895213", Savings = "$8,862", Checking = "$25"}},
            {"C895214", new CustDetails { CustID = "C895214", Savings = "$9,348", Checking = "$103"}},
            // Add more mappings as needed
        };

        /// <summary>
        /// Get Savings Account balance for a Customer by Customer ID.
        /// </summary>
        /// <response code="200">Returns Savings Account balance.</response>
        /// <response code="400">The CustID field is missing or not accepted.</response>
        /// 
        [HttpGet("getsavings")]
        public IActionResult GetSavingsInfoByCustID([FromQuery] string CustID)
        {
            // Manually validate the CustID using CustDetails validation attributes
            var CustDetails = new CustDetails { CustID = CustID };

            // Specify the member name to validate (CustID in this case)
            var validationContext = new ValidationContext(CustDetails, serviceProvider: null, items: null)
            {
                MemberName = nameof(CustDetails.CustID)
            };

            var validationResults = new List<ValidationResult>();

            // Validate only the CustID property
            if (!Validator.TryValidateObject(CustDetails, validationContext, validationResults, validateAllProperties: true))
            {
                // Validation failed for CustID
                var validationErrors = validationResults.Select(result => result.ErrorMessage);
                return BadRequest(validationErrors);
            }

            if (CustDictionary.TryGetValue(CustID, out var matchingCust))
            {
                var result = new
                {
                    CustID = matchingCust.CustID,
                    Savings = matchingCust.Savings
                };

                return Ok(result);
            }

            var noCust = new
            {
                MessDOB = $"No Customer found with the ID '{CustID}'."
            };

            return NotFound(noCust);
        }

        /// <summary>
        /// Get Checking Account balance for a Customer by Customer ID.
        /// </summary>
        /// <response code="200">Returns Checking Account balance.</response>
        /// <response code="400">The CustID field is missing or not accepted.</response>
        /// 
        [HttpGet("getchecking")]
        public IActionResult GetCheckingInfoByCustID([FromQuery] string CustID)
        {
            // Manually validate the CustID using CustDetails validation attributes
            var CustDetails = new CustDetails { CustID = CustID };

            // Specify the member name to validate (CustID in this case)
            var validationContext = new ValidationContext(CustDetails, serviceProvider: null, items: null)
            {
                MemberName = nameof(CustDetails.CustID)
            };

            var validationResults = new List<ValidationResult>();

            // Validate only the CustID property
            if (!Validator.TryValidateObject(CustDetails, validationContext, validationResults, validateAllProperties: true))
            {
                // Validation failed for CustID
                var validationErrors = validationResults.Select(result => result.ErrorMessage);
                return BadRequest(validationErrors);
            }


            if (CustDictionary.TryGetValue(CustID, out var matchingCust))
            {
                var result = new
                {
                    CustID = matchingCust.CustID,
                    Checking = matchingCust.Checking
                };

                return Ok(result);
            }

            var noCust = new
            {
                MessDOB = $"No Customer found with the ID '{CustID}'."
            };

            return NotFound(noCust);
        }

        public class CustDetails
        {
            [RegularExpression("^[a-zA-Z][0-9]{6}$", ErrorMessage = "CustID must start with a letter followed by a 6-digit number")]
            public string CustID { get; set; }

            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric")]
            public string Savings { get; set; }

            [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "LastName must be alphanumeric")]
            public string Checking { get; set; }
        }
    }
}