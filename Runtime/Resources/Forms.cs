using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Forms
    {
        private readonly VeilMailHttpClient _http;

        internal Forms(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/forms";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string formId)
        {
            return _http.GetAsync($"/forms/{formId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/forms", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string formId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/forms/{formId}", parameters);
        }

        public async Task DeleteAsync(string formId)
        {
            await _http.DeleteAsync($"/forms/{formId}");
        }
    }
}
