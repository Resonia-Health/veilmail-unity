namespace VeilMail.Exceptions
{
    public class NotFoundException : VeilMailException
    {
        public NotFoundException(string message = "Resource not found")
            : base(message, 404, "not_found") { }
    }
}
