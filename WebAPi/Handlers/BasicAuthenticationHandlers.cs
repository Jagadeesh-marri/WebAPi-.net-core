using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebAPi.Models;

namespace WebAPi.Handlers
{
    public class BasicAuthenticationHandlers : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly webapiContext _webapi_context;
        public BasicAuthenticationHandlers(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock,
            webapiContext webapi_context
            ) : base(options, loggerFactory, urlEncoder, systemClock)
        {
            _webapi_context = webapi_context;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                    return AuthenticateResult.Fail("Authorization header was not found");
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                var emailaddress = credentials[0];
                var password = credentials[1];
                User user = _webapi_context.User.Where(user => user.EmailAddress == emailaddress && user.Password == password).FirstOrDefault();
                if (user == null)
                {
                    AuthenticateResult.Fail("Invalid username or password");
                }
                else
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name,user.EmailAddress),
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Error has occured");
            }
            return AuthenticateResult.Fail("Need to implement");
            //throw new NotImplementedException();
        }
    }
}
