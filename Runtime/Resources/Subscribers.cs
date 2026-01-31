using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    /// <summary>
    /// Sub-resource for managing subscribers within an audience.
    /// Accessed via Audiences.Subscribers.
    /// </summary>
    public class Subscribers
    {
        private readonly VeilMailHttpClient _http;

        internal Subscribers(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string audienceId, string cursor = null, int? limit = null)
        {
            var query = $"/audiences/{audienceId}/subscribers";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string audienceId, string subscriberId)
        {
            return _http.GetAsync($"/audiences/{audienceId}/subscribers/{subscriberId}");
        }

        public Task<Dictionary<string, object>> AddAsync(string audienceId, Dictionary<string, object> parameters)
        {
            return _http.PostAsync($"/audiences/{audienceId}/subscribers", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string audienceId, string subscriberId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/audiences/{audienceId}/subscribers/{subscriberId}", parameters);
        }

        public async Task RemoveAsync(string audienceId, string subscriberId)
        {
            await _http.DeleteAsync($"/audiences/{audienceId}/subscribers/{subscriberId}");
        }
    }
}
