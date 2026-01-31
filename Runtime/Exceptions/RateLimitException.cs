namespace VeilMail.Exceptions
{
    public class RateLimitException : VeilMailException
    {
        public int? RetryAfter { get; }

        public RateLimitException(string message = "Rate limit exceeded", int? retryAfter = null)
            : base(message, 429, "rate_limit")
        {
            RetryAfter = retryAfter;
        }
    }
}
