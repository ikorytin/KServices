using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using KServices.Core.Domain.Services.Helpers;
using NLog;

namespace KServices.Core.Domain.Services
{
    /// <summary>
    /// The server logger.
    /// </summary>
    public class ServerLogger
    {
        #region Constants and Fields

        /// <summary>
        /// The _logger.
        /// </summary>
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public Methods

        /// <summary>
        /// The log error.
        /// </summary>
        /// <param name="httpContext">
        /// The http context.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="showErrorView">
        /// The show error view.
        /// </param>
        public static void LogError(HttpContext httpContext, HttpRequest request, bool showErrorView = true)
        {
            // we don't need to log this error
            if (httpContext.Request.FilePath.Contains("favicon.ico"))
            {
                return;
            }

            // Get the error details
            Exception lastError = httpContext.Server.GetLastError();
            var error = new KeyValuePair<string, object>("ErrorMessage", lastError.ToString());
            httpContext.Response.Clear();

            ReportUnhandledException(lastError, request);
            if (showErrorView)
            {
                ShowErrorView(httpContext, error);
            }
        }

        /// <summary>
        /// The log error message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            _logger.Error(message);
        }

        /// <summary>
        /// The log error message.
        /// </summary>
        /// <param name="exception"> Exception to log </param>
        /// <param name="message"> Message to write in the log</param>        
        public static void LogException(Exception exception, string message = "")
        {
            _logger.ErrorException(message, exception);
        }

        /// <summary>
        /// The log message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogAction(string message)
        {
            _logger.Info(message);
        }

        public static void LogAction(string message, params object[] args)
        {
            _logger.Info(string.Format(message, args));
        }

        /// <summary>
        /// Report unhandled exceptions
        /// </summary>
        /// <param name="lastErrorWrapper">
        /// </param>
        /// <param name="absoluteUrl"></param>
        public static void ReportUnhandledException(Exception lastErrorWrapper, string absoluteUrl)
        {
            Exception lastServerError = ExtractInnerException(lastErrorWrapper);

            // log all the cases of unhandled exceptions
            LogError(absoluteUrl, lastServerError);
        }

        /// <summary>
        /// The show error view.
        /// </summary>
        /// <param name="ctx">
        /// The HttpContext.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="errorViewName">
        /// The error view name.
        /// </param>
        public static void ShowErrorView(
            HttpContext ctx, KeyValuePair<string, object> error, string errorViewName = "Error")
        {
            if (ctx.CurrentHandler is MvcHandler)
            {
                RequestContext rc = ((MvcHandler)ctx.CurrentHandler).RequestContext;
                string controllerName = rc.RouteData.GetRequiredString("controller");

                IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                IController controller = factory.CreateController(rc, controllerName);
                var cc = new ControllerContext(rc, (ControllerBase)controller);

                var viewResult = new ViewResult { ViewName = errorViewName };
                viewResult.ViewData.Add(error);
                viewResult.ExecuteResult(cc);
                ctx.Server.ClearError();
            }
        }

        public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception exception = (Exception)args.ExceptionObject;            
            Exception lastError = ExtractInnerException(exception);
            
            LogError(lastError);
        }        

        #endregion

        #region Methods

        /// <summary>
        /// The extract inner exception.
        /// </summary>
        /// <param name="lastServerError">
        /// The last server error.
        /// </param>
        /// <returns>
        /// </returns>
        private static Exception ExtractInnerException(Exception lastServerError)
        {
            Exception inner;
            if ((inner = lastServerError.InnerException) == null)
            {
                return lastServerError;
            }

            // often errors are returned wrapped into target invocation exception
            // get the real exception
            if (inner is TargetInvocationException)
            {
                inner = inner.InnerException;
            }

            return inner;
        }

        /// <summary>
        /// Logs the error to file
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="lastError">
        /// </param>
        private static void LogError(string source, Exception lastError)
        {
            _logger.ErrorException(string.Format("Unhandled Exception caught in {0}.", source), lastError);
        }

        /// <summary>
        /// Logs the error to file
        /// </summary>                
        /// <param name="lastError">
        /// </param>
        private static void LogError(Exception lastError)
        {
            _logger.ErrorException("Unhandled Exception caught.", lastError);
        }

        /// <summary>
        /// Report unhandled exceptions
        /// </summary>
        /// <param name="lastErrorWrapper">
        /// </param>
        /// <param name="request">
        /// </param>
        private static void ReportUnhandledException(Exception lastErrorWrapper, HttpRequest request)
        {
            ReportUnhandledException(lastErrorWrapper, request.RawUrl.ToAbsoluteUrl());
        }

        #endregion
    }
}