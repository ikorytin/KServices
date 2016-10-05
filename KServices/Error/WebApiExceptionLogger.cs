using System.Web.Http.ExceptionHandling;

namespace KServices.Error
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            base.Log(context);
            var ex = context.Exception;
            NLog.LogManager.GetCurrentClassLogger().Log(NLog.LogLevel.Error, "Global", ex);
        }
    }
}