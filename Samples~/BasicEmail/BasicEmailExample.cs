using System.Collections.Generic;
using UnityEngine;
using VeilMail;

public class BasicEmailExample : MonoBehaviour
{
    [SerializeField] private VeilMailConfig config;

    async void Start()
    {
        var client = new VeilMailClient(config);

        try
        {
            var result = await client.Emails.SendAsync(new Dictionary<string, object>
            {
                ["from"] = "hello@yourdomain.com",
                ["to"] = "user@example.com",
                ["subject"] = "Hello from Unity!",
                ["html"] = "<h1>Welcome!</h1><p>This email was sent from a Unity project.</p>",
            });

            Debug.Log($"Email sent! ID: {result["id"]}");
        }
        catch (VeilMail.Exceptions.VeilMailException ex)
        {
            Debug.LogError($"VeilMail error: {ex.Message} (code: {ex.ErrorCode})");
        }
    }
}
