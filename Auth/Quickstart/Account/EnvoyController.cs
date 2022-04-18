using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Auth.Quickstart.Account
{
    [SecurityHeaders]
    //[Authorize]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class EnvoyController : ControllerBase
    {
        [HttpGet("{**catchAll}")]
        public IActionResult Get()
        {
            Console.WriteLine("hit IsAuthenticated endpoint");

            Console.WriteLine("UserName: " + User?.GetDisplayName());

            Request.Headers.Add(new KeyValuePair<string, StringValues>("x-current-user", "Daniil Nichitenco"));

            return Ok("bla bla bla");
        }
    }
}
