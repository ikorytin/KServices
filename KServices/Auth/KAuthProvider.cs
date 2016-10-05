using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using KServices.Core.Domain.Services;

namespace Needles48.Web.Auth
{
    public class KAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly IAuthentication _authentication;

        public const string Issuer = "KK99";

        public KAuthProvider(IAuthentication authentication)
        {
            _authentication = authentication;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        #pragma warning disable 1998

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            if (string.IsNullOrWhiteSpace(context.UserName) || string.IsNullOrWhiteSpace(context.Password))
            {
                context.SetError("username and password are required.");
                return;
            }

            var authenticated = _authentication.Authenticate(context.UserName, context.Password);
            if (!authenticated)
            {
                context.SetError("username or password is invalid.");
                return;
            }

            ClaimsIdentity identity =
                new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", context.UserName));
            identity.AddClaim(new Claim("roles", "not implemented"));

            var ticket = new AuthenticationTicket(identity, null);

            context.Validated(ticket);
        }

        #pragma warning restore 1998
    }
}