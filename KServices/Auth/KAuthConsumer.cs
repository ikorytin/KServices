using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KServices.Core.Domain.Services;
using Microsoft.Owin.Security.OAuth;
using Needles48.Web.Auth;

namespace KServices.Auth
{
    public class KAuthConsumer : OAuthBearerAuthenticationProvider
    {
        private readonly IAuthentication _authentication;

        public KAuthConsumer(IAuthentication authentication)
        {
            _authentication = authentication;
        }
        
        public override Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            var app = context.Ticket.Identity.Claims.SingleOrDefault(x => x.Type == AppPassword.KeyClaims);
            if (app == null || !AppPassword.Validate(app.Value))
            {
                context.Rejected();
            }

            Claim userClaim = context.Ticket.Identity.Claims.SingleOrDefault(x => x.Type == "username");
            if (userClaim == null || !_authentication.Authenticate(userClaim.Value))
            {
                context.Rejected();
            }

            return base.ValidateIdentity(context);
        }
    }
}