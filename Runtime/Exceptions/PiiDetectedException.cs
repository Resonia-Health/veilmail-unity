using System.Collections.Generic;

namespace VeilMail.Exceptions
{
    public class PiiDetectedException : VeilMailException
    {
        public List<string> PiiTypes { get; }

        public PiiDetectedException(string message = "PII detected in email content", List<string> piiTypes = null)
            : base(message, 422, "pii_detected")
        {
            PiiTypes = piiTypes ?? new List<string>();
        }
    }
}
