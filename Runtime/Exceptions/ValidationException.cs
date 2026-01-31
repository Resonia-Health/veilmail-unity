namespace VeilMail.Exceptions
{
    public class ValidationException : VeilMailException
    {
        public ValidationException(string message = "Validation error")
            : base(message, 400, "validation_error") { }
    }
}
