using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Feeds
    {
        private readonly VeilMailHttpClient _http;

        internal Feeds(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/feeds";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string feedId)
        {
            return _http.GetAsync($"/feeds/{feedId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/feeds", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string feedId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/feeds/{feedId}", parameters);
        }

        public async Task DeleteAsync(string feedId)
        {
            await _http.DeleteAsync($"/feeds/{feedId}");
        }
    }
}
