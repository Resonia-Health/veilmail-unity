using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Domains
    {
        private readonly VeilMailHttpClient _http;

        internal Domains(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/domains";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string domainId)
        {
            return _http.GetAsync($"/domains/{domainId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(string domain)
        {
            return _http.PostAsync("/domains", new Dictionary<string, object>
            {
                ["domain"] = domain
            });
        }

        public Task<Dictionary<string, object>> VerifyAsync(string domainId)
        {
            return _http.PostAsync($"/domains/{domainId}/verify", new Dictionary<string, object>());
        }

        public async Task DeleteAsync(string domainId)
        {
            await _http.DeleteAsync($"/domains/{domainId}");
        }
    }
}
