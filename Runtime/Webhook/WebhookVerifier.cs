using System;
using System.Security.Cryptography;
using System.Text;

namespace VeilMail.Webhook
{
    public static class WebhookVerifier
    {
        public static bool Verify(string payload, string signature, string secret)
        {
            if (string.IsNullOrEmpty(payload) || string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(secret))
                return false;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var computed = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return string.Equals(computed, signature, StringComparison.OrdinalIgnoreCase);
        }
    }
}
