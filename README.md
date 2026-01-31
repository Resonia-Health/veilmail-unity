# VeilMail SDK for Unity

Official Veil Mail SDK for Unity. Send transactional emails with automatic PII protection directly from your Unity projects.

## Overview

The VeilMail Unity SDK provides full access to the Veil Mail API from Unity 2021.3 and later. It uses `UnityWebRequest` under the hood for cross-platform compatibility (Editor, Standalone, Mobile, WebGL) and offers both async/await and coroutine APIs.

### Features

- Full API coverage: Emails, Domains, Templates, Audiences, Campaigns, Webhooks, Topics, Properties, Sequences, Feeds, Forms, Analytics
- Dual API: async/await for modern C# and coroutine helpers for MonoBehaviour workflows
- Editor tools: Project Settings panel for configuration, Test Email window for quick testing
- VeilMailConfig ScriptableObject with automatic API key stripping for builds
- Webhook signature verification
- Cross-platform support via UnityWebRequest (no System.Net.Http dependency)
- Zero external dependencies -- includes a built-in lightweight JSON parser

## Installation

### Unity Package Manager (Git URL)

1. Open Unity and go to **Window > Package Manager**.
2. Click the **+** button in the top-left corner.
3. Select **Add package from git URL...**
4. Enter the following URL:

```
https://github.com/Resonia-Health/veilmail-unity.git
```

5. Click **Add**.

### Manual Installation

1. Clone or download this repository.
2. Copy the `packages/sdk-unity` folder into your Unity project's `Packages/` directory.
3. Rename it to `xyz.veilmail.sdk`.

## Quick Start

### 1. Configure Your API Key

Go to **Edit > Project Settings > VeilMail** and enter your API key. You can obtain an API key from the [VeilMail Dashboard](https://veilmail.xyz).

### 2. Send an Email (async/await)

```csharp
using System.Collections.Generic;
using UnityEngine;
using VeilMail;

public class EmailSender : MonoBehaviour
{
    async void Start()
    {
        var client = VeilMailClient.FromSettings();

        var result = await client.Emails.SendAsync(new Dictionary<string, object>
        {
            ["from"] = "hello@yourdomain.com",
            ["to"] = "user@example.com",
            ["subject"] = "Hello from Unity!",
            ["html"] = "<h1>Welcome!</h1><p>Sent from Unity.</p>",
        });

        Debug.Log($"Email sent! ID: {result["id"]}");
    }
}
```

### 3. Send an Email (coroutine)

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VeilMail;
using VeilMail.Utilities;

public class EmailSenderCoroutine : MonoBehaviour
{
    IEnumerator Start()
    {
        var client = VeilMailClient.FromSettings();

        yield return client.Emails.SendAsync(new Dictionary<string, object>
        {
            ["from"] = "hello@yourdomain.com",
            ["to"] = "user@example.com",
            ["subject"] = "Hello from Unity!",
            ["html"] = "<h1>Welcome!</h1>",
        }).AsCoroutine<Dictionary<string, object>>(
            onSuccess: result => Debug.Log($"Sent! ID: {result["id"]}"),
            onError: ex => Debug.LogError($"Failed: {ex.Message}")
        );
    }
}
```

## Editor Tools

### Project Settings

Navigate to **Edit > Project Settings > VeilMail** to configure:

- **API Key** -- Your Veil Mail API key (masked by default).
- **Base URL** -- API endpoint (default: `https://api.veilmail.xyz`).
- **Timeout** -- Request timeout in seconds (default: 30).
- **Strip API Key in Builds** -- When enabled (default), the API key is removed from build artifacts for security.
- **Test Connection** -- Verify your API key works with a single click.

### Test Email Window

Go to **Tools > VeilMail > Send Test Email** to open a window where you can send test emails directly from the editor without writing any code.

## Security

**Important:** Never ship builds with your API key embedded. The SDK includes a `stripKeyInBuilds` option (enabled by default) that removes the API key from VeilMailConfig when building.

For production builds, use the **Server Proxy** pattern: run a lightweight server that holds your API key and have the Unity client communicate through it. See the Server Proxy sample for a complete example.

Refer to `Documentation~/SECURITY.md` for full security guidance.

## API Reference Summary

All resources are accessed through the `VeilMailClient` instance:

| Resource | Methods |
|---|---|
| `client.Emails` | SendAsync, GetAsync, ListAsync, CancelAsync |
| `client.Domains` | ListAsync, GetAsync, CreateAsync, VerifyAsync, DeleteAsync |
| `client.Templates` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync |
| `client.Audiences` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync, Subscribers (sub-resource) |
| `client.Campaigns` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync, SendAsync, ScheduleAsync |
| `client.Webhooks` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync |
| `client.Topics` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync |
| `client.Properties` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync |
| `client.Sequences` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync, ActivateAsync, PauseAsync |
| `client.Feeds` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync |
| `client.Forms` | ListAsync, GetAsync, CreateAsync, UpdateAsync, DeleteAsync |
| `client.Analytics` | GetOverviewAsync, GetEmailStatsAsync, GetCampaignStatsAsync |

All methods return `Task<Dictionary<string, object>>` and can be used with both async/await and coroutines (via the `AsCoroutine` extension method).

### Webhook Verification

```csharp
using VeilMail.Webhook;

bool isValid = WebhookVerifier.Verify(payload, signature, webhookSecret);
```

## Samples

Import samples via **Window > Package Manager > VeilMail SDK > Samples**:

- **Basic Email** -- Minimal email sending example.
- **Template Email** -- Sending emails using templates with dynamic data.
- **Server Proxy** -- Secure proxy pattern for shipped builds (recommended for production).

## Requirements

- Unity 2021.3 or later
- .NET Standard 2.1 or .NET Framework 4.x scripting backend

## Support

- Documentation: [https://veilmail.xyz/docs/sdk-unity](https://veilmail.xyz/docs/sdk-unity)
- Issues: [https://github.com/Resonia-Health/veilmail-unity/issues](https://github.com/Resonia-Health/veilmail-unity/issues)
- Email: support@veilmail.xyz

## License

MIT License. See [LICENSE](LICENSE) for details.
