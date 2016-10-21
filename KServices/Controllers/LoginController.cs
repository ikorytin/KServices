using System;
using System.Security.Claims;
using System.Web.Http;
using KServices.Auth;
using KServices.Core.Domain.Core.Exceptions;
using KServices.Core.Domain.Services;
using KServices.Models.Login;
using Microsoft.Owin.Security;
using Needles48.Web.Auth;

namespace KServices.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : BaseApiController
    {
        private readonly IAuthentication _authenticationService;

        public LoginController(IAuthentication authenticationService)
        {

            _authenticationService = authenticationService;
        }

        [Route("")]
        public string Post([FromBody] LoginModel login)
        {
            if (string.IsNullOrEmpty(login.UserName))
            {
                throw new BadParameterException("Username is required.");
            }

            if (string.IsNullOrEmpty(login.Password))
            {
                throw new BadParameterException("Needles password is required.");
            }

            var accountExist = _authenticationService.Authenticate(login.UserName, login.Password);
            if (!accountExist)
            {
                throw new BadParameterException("Username or Password is invalid.");
            }

            var identity = new ClaimsIdentity("Bearer");
            identity.AddClaim(new Claim("username", login.UserName));
            identity.AddClaim(new Claim("roles", "not implemented"));

            var ticket = new AuthenticationTicket(identity, null);
            var now = DateTimeOffset.Now;
            ticket.Properties.IssuedUtc = now;
            ticket.Properties.ExpiresUtc = now.AddDays(1);
            return new JwtFormat(KAuthProvider.Issuer).Protect(ticket);
        }
    }
}