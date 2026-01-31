using System.Collections.Generic;
using UnityEngine;
using VeilMail;

public class TemplateEmailExample : MonoBehaviour
{
    [SerializeField] private VeilMailConfig config;
    [SerializeField] private string templateId = "template_xxxxx";

    async void Start()
    {
        var client = new VeilMailClient(config);

        try
        {
            var result = await client.Emails.SendAsync(new Dictionary<string, object>
            {
                ["from"] = "hello@yourdomain.com",
                ["to"] = "user@example.com",
                ["templateId"] = templateId,
                ["templateData"] = new Dictionary<string, object>
                {
                    ["playerName"] = "Player1",
                    ["score"] = 9001,
                },
            });

            Debug.Log($"Template email sent! ID: {result["id"]}");
        }
        catch (VeilMail.Exceptions.VeilMailException ex)
        {
            Debug.LogError($"VeilMail error: {ex.Message}");
        }
    }
}
