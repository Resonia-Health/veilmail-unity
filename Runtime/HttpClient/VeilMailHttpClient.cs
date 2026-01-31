using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VeilMail.Http
{
    public class VeilMailHttpClient
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly int _timeoutSeconds;
        private readonly IVeilMailTransport _transport;

        public VeilMailHttpClient(string apiKey, string baseUrl, int timeoutSeconds)
        {
            _apiKey = apiKey;
            _baseUrl = baseUrl.TrimEnd('/');
            _timeoutSeconds = timeoutSeconds;
            _transport = new UnityWebRequestTransport();
        }

        public async Task<VeilMailResponse> RequestAsync(HttpMethod method, string path, string body = null)
        {
            var request = new VeilMailRequest
            {
                Method = method,
                Url = $"{_baseUrl}/v1{path}",
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Bearer {_apiKey}",
                    ["Content-Type"] = "application/json",
                    ["Accept"] = "application/json",
                    ["User-Agent"] = "veilmail-unity/0.1.0"
                }
            };

            var response = await _transport.SendAsync(request);

            if (!response.IsSuccess)
                VeilMail.Exceptions.ExceptionFactory.ThrowForStatus(response);

            return response;
        }

        public async Task<Dictionary<string, object>> GetAsync(string path)
        {
            var response = await RequestAsync(HttpMethod.GET, path);
            return VeilMailJsonUtility.Deserialize(response.Body);
        }

        public async Task<Dictionary<string, object>> PostAsync(string path, Dictionary<string, object> data)
        {
            var body = VeilMailJsonUtility.Serialize(data);
            var response = await RequestAsync(HttpMethod.POST, path, body);
            return VeilMailJsonUtility.Deserialize(response.Body);
        }

        public async Task<Dictionary<string, object>> PutAsync(string path, Dictionary<string, object> data)
        {
            var body = VeilMailJsonUtility.Serialize(data);
            var response = await RequestAsync(HttpMethod.PUT, path, body);
            return VeilMailJsonUtility.Deserialize(response.Body);
        }

        public async Task<Dictionary<string, object>> PatchAsync(string path, Dictionary<string, object> data)
        {
            var body = VeilMailJsonUtility.Serialize(data);
            var response = await RequestAsync(HttpMethod.PATCH, path, body);
            return VeilMailJsonUtility.Deserialize(response.Body);
        }

        public async Task DeleteAsync(string path)
        {
            await RequestAsync(HttpMethod.DELETE, path);
        }
    }
}
