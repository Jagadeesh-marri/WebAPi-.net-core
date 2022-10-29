using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebAPi.Handlers
{
    public class BasicAuthenticationHandlers : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandlers(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder,
            ISystemClock systemClock
            ) : base(options, loggerFactory, urlEncoder, systemClock)
        {

        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
             return AuthenticateResult.Fail("Need to implement");
            //throw new NotImplementedException();
        }
    }
}
