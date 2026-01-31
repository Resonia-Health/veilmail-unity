namespace VeilMail.Exceptions
{
    public class AuthenticationException : VeilMailException
    {
        public AuthenticationException(string message = "Invalid API key")
            : base(message, 401, "authentication_error") { }
    }
}
