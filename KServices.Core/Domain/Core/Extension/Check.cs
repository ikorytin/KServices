using System;

namespace KServices.Core.Domain.Core.Extension
{
    public static class Check
    {
        #region Public Methods

        /// <summary>
        /// Requires the specified assertion.        
        /// </summary>
        /// <typeparam name="T">Is type of message</typeparam>
        /// <param name="assertion">if assertion is <c>false</c> throws T message.</param>
        /// <param name="message">The message.</param>
        public static void Require<T>(bool assertion, string message) where T : Exception
        {
            if (!assertion)
            {
                throw (T)typeof(T).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { message });
            }
        }

        /// <summary>
        /// Requires the specified assertion.        
        /// </summary>
        /// <typeparam name="T">Is type of message</typeparam>
        /// <param name="assertion">if assertion is <c>false</c> throws T message.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters"></param>
        public static void Require<T>(bool assertion, string message, params object[] parameters) where T : Exception
        {
            if (!assertion)
            {
                throw (T)typeof(T).GetConstructor(new[] { typeof(string) }).Invoke(new object[] { string.Format(message, parameters) });
            }
        }

        #endregion
    }
}