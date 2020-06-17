using System;

namespace Cirdan.Models
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
        :base(message)
        {
        }
    }
}
