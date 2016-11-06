using System;
using System.Web;

namespace KServices.Core.Domain.Services.Helpers
{    
    /// <summary>
    /// Privet some helpful methods to operate with url
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Gets the application host.
        /// </summary>
        /// <returns>Application host address.</returns>
        public static string GetApplicationHost(bool useSSL)
        {
            string result = "http://" + ServerVariables("HTTP_HOST");

            if (useSSL)
            {
                result = result.Replace("http:/", "https:/");
                result = result.Replace("www.www", "www");
            }

            return result;
        }

        /// <summary>
        /// Gets absolute url from application relative url.
        /// </summary>
        /// <param name="url">relative url without '~'</param>
        /// <param name="useSsl">flag showing need of SSL usage</param>
        /// <returns>Absolute url.</returns>
        public static string ToAbsoluteUrl(this string url, Boolean useSsl = false)
        {
            return GetApplicationHost(useSsl) + url;
        }

        /// <summary>
        /// Gets server variable.
        /// </summary>
        /// <param name="name">Server variable.</param>
        public static string ServerVariables(string name)
        {
            string value = String.Empty;

            try
            {
                if (HttpContext.Current.Request.ServerVariables[name] != null)
                {
                    value = HttpContext.Current.Request.ServerVariables[name];                    
                }
            }
            catch
            {
                value = String.Empty;
            }

            return value;
        }
    }
}