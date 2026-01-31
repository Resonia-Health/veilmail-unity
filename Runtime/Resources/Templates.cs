using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Templates
    {
        private readonly VeilMailHttpClient _http;

        internal Templates(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/templates";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string templateId)
        {
            return _http.GetAsync($"/templates/{templateId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/templates", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string templateId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/templates/{templateId}", parameters);
        }

        public async Task DeleteAsync(string templateId)
        {
            await _http.DeleteAsync($"/templates/{templateId}");
        }
    }
}
