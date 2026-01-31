using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Webhooks
    {
        private readonly VeilMailHttpClient _http;

        internal Webhooks(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/webhooks";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string webhookId)
        {
            return _http.GetAsync($"/webhooks/{webhookId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/webhooks", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string webhookId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/webhooks/{webhookId}", parameters);
        }

        public async Task DeleteAsync(string webhookId)
        {
            await _http.DeleteAsync($"/webhooks/{webhookId}");
        }
    }
}
