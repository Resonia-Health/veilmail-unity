using System.Collections.Generic;
using System.Threading.Tasks;

namespace VeilMail.Http
{
    public enum HttpMethod { GET, POST, PUT, PATCH, DELETE }

    public class VeilMailRequest
    {
        public HttpMethod Method;
        public string Url;
        public string Body;
        public Dictionary<string, string> Headers;
    }

    public class VeilMailResponse
    {
        public int StatusCode;
        public string Body;
        public Dictionary<string, string> Headers;
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
    }

    public interface IVeilMailTransport
    {
        Task<VeilMailResponse> SendAsync(VeilMailRequest request);
    }
}
