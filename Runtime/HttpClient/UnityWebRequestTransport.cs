using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace VeilMail.Http
{
    public class UnityWebRequestTransport : IVeilMailTransport
    {
        public async Task<VeilMailResponse> SendAsync(VeilMailRequest request)
        {
            var uwr = new UnityWebRequest(request.Url, request.Method.ToString());
            uwr.downloadHandler = new DownloadHandlerBuffer();

            if (!string.IsNullOrEmpty(request.Body))
            {
                var bodyBytes = Encoding.UTF8.GetBytes(request.Body);
                uwr.uploadHandler = new UploadHandlerRaw(bodyBytes);
            }

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                    uwr.SetRequestHeader(header.Key, header.Value);
            }

            var operation = uwr.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            var response = new VeilMailResponse
            {
                StatusCode = (int)uwr.responseCode,
                Body = uwr.downloadHandler?.text ?? "",
                Headers = new Dictionary<string, string>()
            };

            var responseHeaders = uwr.GetResponseHeaders();
            if (responseHeaders != null)
            {
                foreach (var header in responseHeaders)
                    response.Headers[header.Key] = header.Value;
            }

            uwr.Dispose();
            return response;
        }
    }
}
