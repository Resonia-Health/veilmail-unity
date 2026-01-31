using System;

namespace VeilMail.Exceptions
{
    public class VeilMailException : Exception
    {
        public int StatusCode { get; }
        public string ErrorCode { get; }

        public VeilMailException(string message, int statusCode = 0, string errorCode = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
