using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace KServices.Error
{
    public class JsonErrorResult : ExceptionResult
    {
        public string Content { get; set; }

        public JsonErrorResult(
            Exception exception,
            bool includeErrorDetail,
            IContentNegotiator negotiator,
            HttpRequestMessage request,
            IEnumerable<MediaTypeFormatter> formatters) :
          base(exception, includeErrorDetail, negotiator, request, formatters)
        {
        }

        /// Creates an HttpResponseMessage instance asynchronously.
        /// This method determines how a HttpResponseMessage content will look like.
        public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var content = new HttpError(Exception, IncludeErrorDetail);
            if (!IncludeErrorDetail)
            {
                content.Message = Exception.Message;
            }

            // define an additional content field with name "ErrorID"
            //content.Add("ErrorID", 101);

            var result =
              ContentNegotiator.Negotiate(typeof(HttpError), Request, Formatters);

            var statuscode = HttpStatusCode.InternalServerError;

            var message = new HttpResponseMessage
            {
                RequestMessage = Request,
                StatusCode = result == null ? HttpStatusCode.NotAcceptable : statuscode
            };

            if (result != null)
            {
                try
                {
                    // serializes the HttpError instance either to JSON or to XML
                    // depend on requested by the client MIME type.
                    message.Content = new ObjectContent<HttpError>(
                      content,
                      result.Formatter,
                      result.MediaType);
                }
                catch
                {
                    message.Dispose();

                    throw;
                }
            }

            return Task.FromResult(message);
        }
    }
}