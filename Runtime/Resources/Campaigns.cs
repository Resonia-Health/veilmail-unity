using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Campaigns
    {
        private readonly VeilMailHttpClient _http;

        internal Campaigns(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/campaigns";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string campaignId)
        {
            return _http.GetAsync($"/campaigns/{campaignId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/campaigns", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string campaignId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/campaigns/{campaignId}", parameters);
        }

        public async Task DeleteAsync(string campaignId)
        {
            await _http.DeleteAsync($"/campaigns/{campaignId}");
        }

        public Task<Dictionary<string, object>> SendAsync(string campaignId)
        {
            return _http.PostAsync($"/campaigns/{campaignId}/send", new Dictionary<string, object>());
        }

        public Task<Dictionary<string, object>> ScheduleAsync(string campaignId, Dictionary<string, object> parameters)
        {
            return _http.PostAsync($"/campaigns/{campaignId}/schedule", parameters);
        }
    }
}
