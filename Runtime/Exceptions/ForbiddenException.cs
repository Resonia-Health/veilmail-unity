namespace VeilMail.Exceptions
{
    public class ForbiddenException : VeilMailException
    {
        public ForbiddenException(string message = "Access denied")
            : base(message, 403, "forbidden") { }
    }
}
