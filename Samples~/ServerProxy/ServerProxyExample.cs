using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Example: Send emails through a server proxy instead of embedding the API key in the build.
/// This pattern is REQUIRED for shipped games and WebGL builds.
/// See the README in this sample for the proxy server setup.
/// </summary>
public class ServerProxyExample : MonoBehaviour
{
    [SerializeField] private string proxyUrl = "http://localhost:3001/api/send-email";

    public async void SendEmailViaProxy(string to, string subject, string body)
    {
        var json = VeilMail.Http.VeilMailJsonUtility.Serialize(new Dictionary<string, object>
        {
            ["to"] = to,
            ["subject"] = subject,
            ["html"] = body,
        });

        var request = new UnityWebRequest(proxyUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        var operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log($"Email sent via proxy: {request.downloadHandler.text}");
        else
            Debug.LogError($"Proxy error: {request.error}");

        request.Dispose();
    }
}
