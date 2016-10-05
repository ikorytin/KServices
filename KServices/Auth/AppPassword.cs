using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Needles48.Web.Auth
{
    public class AppPassword
    {
        public const string KeyAppPassword = "password";
        public const string KeyClaims = "app";

        private static string appPassword;

        static AppPassword()
        {
            appPassword = ConfigurationManager.AppSettings[KeyAppPassword];
        }

        public static bool Validate(string userPwd)
        {
            return string.Compare(userPwd, appPassword, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}