namespace VeilMail.Exceptions
{
    public class ServerException : VeilMailException
    {
        public ServerException(string message = "Internal server error")
            : base(message, 500, "server_error") { }
    }
}
