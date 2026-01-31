using System.Collections.Generic;
using VeilMail.Http;

namespace VeilMail.Exceptions
{
    internal static class ExceptionFactory
    {
        public static void ThrowForStatus(VeilMailResponse response)
        {
            var body = VeilMailJsonUtility.Deserialize(response.Body);
            var message = "API request failed";
            var errorCode = "";
            List<string> piiTypes = null;

            if (body.TryGetValue("error", out var errorObj) && errorObj is Dictionary<string, object> error)
            {
                if (error.TryGetValue("message", out var msg))
                    message = msg?.ToString() ?? message;
                if (error.TryGetValue("code", out var code))
                    errorCode = code?.ToString() ?? "";
                if (error.TryGetValue("piiTypes", out var pii) && pii is List<object> piiList)
                {
                    piiTypes = new List<string>();
                    foreach (var p in piiList) piiTypes.Add(p?.ToString() ?? "");
                }
            }

            switch (response.StatusCode)
            {
                case 400: throw new ValidationException(message);
                case 401: throw new AuthenticationException(message);
                case 403: throw new ForbiddenException(message);
                case 404: throw new NotFoundException(message);
                case 422:
                    if (errorCode == "pii_detected")
                        throw new PiiDetectedException(message, piiTypes);
                    throw new ValidationException(message);
                case 429:
                    int? retryAfter = null;
                    if (response.Headers.TryGetValue("Retry-After", out var ra) && int.TryParse(ra, out var raInt))
                        retryAfter = raInt;
                    throw new RateLimitException(message, retryAfter);
                default:
                    if (response.StatusCode >= 500)
                        throw new ServerException(message);
                    throw new VeilMailException(message, response.StatusCode, errorCode);
            }
        }
    }
}
