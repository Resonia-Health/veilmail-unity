# Security Guide

This document covers security best practices for using the VeilMail Unity SDK across different build targets and deployment scenarios.

## Threat Model

API keys grant access to your VeilMail account and allow sending emails on your behalf. If an API key is leaked, an attacker could:

- Send unauthorized emails from your domains
- Access your email analytics and subscriber data
- Exhaust your sending quota

The primary risk in Unity projects is that API keys embedded in builds can be extracted through decompilation, memory inspection, or network traffic analysis.

## Usage Scenarios

### Editor Only (Safe)

When using the SDK exclusively in the Unity Editor (for tooling, automated testing, or internal workflows), it is safe to store your API key in the `VeilMailConfig` ScriptableObject. The key never leaves your development machine.

**Recommended for:**
- Editor scripts and automation
- Internal development tools
- CI/CD pipelines running in headless Unity

**Configuration:**
1. Go to Edit > Project Settings > VeilMail
2. Enter your API key
3. Keep `Strip API Key in Builds` enabled (default)

### Shipped Builds -- Standalone / Console (Proxy Required)

Desktop and console builds can be decompiled. IL2CPP raises the bar slightly compared to Mono, but determined attackers can still extract strings from binaries.

**You must use a server proxy.** The Unity client sends requests to your proxy server, which holds the API key and forwards requests to the VeilMail API.

**Recommended architecture:**
```
Unity Client  -->  Your Proxy Server  -->  VeilMail API
  (no key)         (holds API key)         (authenticates)
```

See the `Samples~/ServerProxy` sample for a complete implementation.

### WebGL Builds (Proxy Mandatory)

WebGL builds are the highest-risk target. All code runs in the browser as JavaScript, and all network traffic is visible in browser developer tools. There is absolutely no way to hide an API key in a WebGL build.

**A server proxy is mandatory for WebGL.** There are no exceptions.

Additionally:
- CORS headers on your proxy must be configured to only allow requests from your game's domain
- Implement rate limiting on your proxy to prevent abuse
- Use session-based authentication between your game and proxy

### Mobile Builds -- iOS / Android (Proxy Recommended)

Mobile builds can be intercepted with proxy tools (Charles, mitmproxy) and decompiled with standard reverse engineering tools. While certificate pinning can make interception harder, it is not a reliable security boundary.

**A server proxy is strongly recommended for mobile builds.**

If you must embed a key for a limited use case (e.g., a controlled beta), use a test API key (`veil_test_xxx`) with restricted permissions, and rotate it frequently.

## VeilMailConfig.stripKeyInBuilds

The `VeilMailConfig` ScriptableObject includes a `stripKeyInBuilds` toggle (enabled by default). When enabled:

- During the build process, the API key field in the `VeilMailConfig` asset included in the build is cleared
- The key remains intact in your project's Assets folder for Editor use
- This provides defense-in-depth: even if a developer forgets to implement the proxy pattern, the key will not be present in the build

**How it works:**
The key is stored in a ScriptableObject loaded from `Resources/VeilMailConfig`. When `stripKeyInBuilds` is true, the build pipeline clears the `apiKey` field from the asset that gets packaged into the build. The original asset in your project remains unchanged.

**Important:** This feature is a safety net, not a security strategy. You should still implement the server proxy pattern for any build that leaves your development environment.

## Recommendations Summary

| Scenario | API Key in Build | Proxy Required | Risk Level |
|---|---|---|---|
| Editor only | Safe | No | None |
| Standalone (internal) | Avoid | Recommended | Medium |
| Standalone (shipped) | Never | Required | High |
| WebGL | Never | Mandatory | Critical |
| Mobile (iOS/Android) | Never | Strongly recommended | High |
| Console | Never | Required | High |

## Additional Security Measures

1. **Use scoped API keys.** Create API keys with the minimum required permissions for your use case.
2. **Rotate keys regularly.** If you suspect a key has been compromised, rotate it immediately in the VeilMail dashboard.
3. **Monitor usage.** Check your VeilMail analytics for unexpected sending patterns that might indicate unauthorized use.
4. **Implement proxy authentication.** Your proxy server should authenticate Unity clients using game session tokens, player IDs, or other application-level credentials.
5. **Rate limit your proxy.** Prevent abuse by limiting how many emails a single client can trigger per time period.
6. **Use webhook signature verification.** When receiving webhooks, always verify the signature using `WebhookVerifier.Verify()` to ensure the payload came from VeilMail.
