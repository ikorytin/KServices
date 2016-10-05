using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace KServices.Error
{
    public class WebApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var requestContext = context.RequestContext;
            var config = requestContext.Configuration;

            context.Result = new JsonErrorResult(
              context.Exception,
              requestContext == null ? false : requestContext.IncludeErrorDetail,
              config.Services.GetContentNegotiator(),
              context.Request,
              config.Formatters);
        }
    }
}