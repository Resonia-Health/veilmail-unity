# Server Proxy Pattern

For shipped game builds, **never embed your API key** in the client. Instead, run a lightweight proxy server that holds the API key.

## Why?

- Game clients can be decompiled — API keys will be extracted
- WebGL builds run in the browser — keys are visible in network traffic
- Mobile builds can be intercepted with proxy tools

## Example Proxy (Express.js)

```javascript
const express = require('express');
const { VeilMail } = require('@resonia/veilmail-sdk');

const app = express();
const veilmail = new VeilMail(process.env.VEILMAIL_API_KEY);

app.post('/api/send-email', express.json(), async (req, res) => {
  // Add your own auth (game session token, etc.)
  const { to, subject, html } = req.body;

  const email = await veilmail.emails.send({
    from: 'game@yourdomain.com',
    to,
    subject,
    html,
  });

  res.json({ id: email.id });
});

app.listen(3001);
```

## Unity Client

Use `ServerProxyExample.cs` in this sample to call the proxy instead of the VeilMail API directly.
