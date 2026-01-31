using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Properties
    {
        private readonly VeilMailHttpClient _http;

        internal Properties(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/properties";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string propertyId)
        {
            return _http.GetAsync($"/properties/{propertyId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/properties", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string propertyId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/properties/{propertyId}", parameters);
        }

        public async Task DeleteAsync(string propertyId)
        {
            await _http.DeleteAsync($"/properties/{propertyId}");
        }
    }
}
