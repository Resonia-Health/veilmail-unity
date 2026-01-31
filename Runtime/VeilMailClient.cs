using System;
using UnityEngine;
using VeilMail.Http;
using VeilMail.Resources;

namespace VeilMail
{
    /// <summary>
    /// Main client for the Veil Mail API.
    /// </summary>
    public class VeilMailClient
    {
        private readonly VeilMailHttpClient _http;

        private Emails _emails;
        private Domains _domains;
        private Templates _templates;
        private Audiences _audiences;
        private Campaigns _campaigns;
        private Webhooks _webhooks;
        private Topics _topics;
        private Properties _properties;
        private Sequences _sequences;
        private Feeds _feeds;
        private Forms _forms;
        private Analytics _analytics;

        /// <summary>Create a client with an API key.</summary>
        public VeilMailClient(string apiKey, string baseUrl = "https://api.veilmail.xyz", int timeoutSeconds = 30)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("API key is required", nameof(apiKey));

            _http = new VeilMailHttpClient(apiKey, baseUrl, timeoutSeconds);
        }

        /// <summary>Create a client from a VeilMailConfig ScriptableObject.</summary>
        public VeilMailClient(VeilMailConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (string.IsNullOrEmpty(config.apiKey))
                throw new ArgumentException("API key is not set in VeilMailConfig");

            _http = new VeilMailHttpClient(config.apiKey, config.baseUrl, config.timeoutSeconds);
        }

        /// <summary>Create a client from Project Settings.</summary>
        public static VeilMailClient FromSettings()
        {
            var config = VeilMailConfig.Instance;
            if (config == null)
                throw new InvalidOperationException("VeilMailConfig not found. Create one via Edit > Project Settings > VeilMail.");
            return new VeilMailClient(config);
        }

        public Emails Emails => _emails ??= new Emails(_http);
        public Domains Domains => _domains ??= new Domains(_http);
        public Templates Templates => _templates ??= new Templates(_http);
        public Audiences Audiences => _audiences ??= new Audiences(_http);
        public Campaigns Campaigns => _campaigns ??= new Campaigns(_http);
        public Webhooks Webhooks => _webhooks ??= new Webhooks(_http);
        public Topics Topics => _topics ??= new Topics(_http);
        public Properties Properties => _properties ??= new Properties(_http);
        public Sequences Sequences => _sequences ??= new Sequences(_http);
        public Feeds Feeds => _feeds ??= new Feeds(_http);
        public Forms Forms => _forms ??= new Forms(_http);
        public Analytics Analytics => _analytics ??= new Analytics(_http);
    }
}
