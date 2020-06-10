using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bank.Simulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bank.Simulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        public TokenController()
        {

        }

        [HttpPost]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        public IActionResult GenerateToken([FromBody]TokenRequest tokenRequest)
        {
            if (!(string.Equals(tokenRequest.ClientId, "string") && string.Equals(tokenRequest.ClientSecret, "string")))
            {
                return Unauthorized();
            }

            return Ok(new TokenResponse()
            {
                Token = Guid.NewGuid().ToString(),
                ValidTo = DateTime.UtcNow.AddMinutes(15)
            });
        }
    }
}
