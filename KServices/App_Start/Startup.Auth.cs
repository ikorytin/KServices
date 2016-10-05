using System;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using KServices.Auth;
using KServices.Core.Domain.Core.Windsor;
using KServices.Core.Domain.Services;
using KServices.Error;
using KServices.Locator;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Needles48.Web.Auth;
using Newtonsoft.Json.Serialization;
using JwtFormat = KServices.Auth.JwtFormat;

namespace KServices
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        private static readonly IWindsorContainer _container = WebIoC.Container;

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            app.UseNLog();
            httpConfig.Services.Replace(typeof(IExceptionHandler), new WebApiExceptionHandler());
            httpConfig.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger());

            ConfigureOAuthTokenGeneration(app);

            ConfigureOAuthTokenConsumption(app);

            ConfigureWebApi(httpConfig);
            
            app.UseWebApi(httpConfig);
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            RegisterDependencyResolver(config);
           
        }

        private void RegisterDependencyResolver(HttpConfiguration config)
        {
            _container.Register(
             Classes
                 .FromThisAssembly()
                 .BasedOn<ApiController>()
                 .LifestyleScoped()
             );

            config.DependencyResolver = new CastleDependencyResolver(_container);
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            var provider = new KAuthProvider(_container.Resolve<IAuthentication>());
            //var provider = new KAuthProvider(() => container.Resolve<IStaffBusinessService>());
            var OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = provider,
                AccessTokenFormat = new JwtFormat(KAuthProvider.Issuer)
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var provider = new KAuthConsumer(_container.Resolve<IAuthentication>());
            var issuer = KAuthProvider.Issuer;
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    },
                    Provider = provider
                });
        }
    }
}
