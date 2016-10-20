using System;

namespace KServices.Core.Domain.Core.Exceptions
{
    public class BadParameterException : Exception
    {
        public BadParameterException(string message) 
            :base(message)
        {
        }
    }
}