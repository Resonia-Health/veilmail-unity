using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Audiences
    {
        private readonly VeilMailHttpClient _http;
        private Subscribers _subscribers;

        internal Audiences(VeilMailHttpClient http) { _http = http; }

        /// <summary>Sub-resource for managing subscribers within audiences.</summary>
        public Subscribers Subscribers => _subscribers ??= new Subscribers(_http);

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/audiences";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string audienceId)
        {
            return _http.GetAsync($"/audiences/{audienceId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/audiences", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string audienceId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/audiences/{audienceId}", parameters);
        }

        public async Task DeleteAsync(string audienceId)
        {
            await _http.DeleteAsync($"/audiences/{audienceId}");
        }
    }
}
