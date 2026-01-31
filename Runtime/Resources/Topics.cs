using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Topics
    {
        private readonly VeilMailHttpClient _http;

        internal Topics(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/topics";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string topicId)
        {
            return _http.GetAsync($"/topics/{topicId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/topics", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string topicId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/topics/{topicId}", parameters);
        }

        public async Task DeleteAsync(string topicId)
        {
            await _http.DeleteAsync($"/topics/{topicId}");
        }
    }
}
