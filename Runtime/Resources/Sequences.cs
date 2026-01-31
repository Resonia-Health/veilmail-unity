using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Sequences
    {
        private readonly VeilMailHttpClient _http;

        internal Sequences(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/sequences";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetAsync(string sequenceId)
        {
            return _http.GetAsync($"/sequences/{sequenceId}");
        }

        public Task<Dictionary<string, object>> CreateAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/sequences", parameters);
        }

        public Task<Dictionary<string, object>> UpdateAsync(string sequenceId, Dictionary<string, object> parameters)
        {
            return _http.PatchAsync($"/sequences/{sequenceId}", parameters);
        }

        public async Task DeleteAsync(string sequenceId)
        {
            await _http.DeleteAsync($"/sequences/{sequenceId}");
        }

        public Task<Dictionary<string, object>> ActivateAsync(string sequenceId)
        {
            return _http.PostAsync($"/sequences/{sequenceId}/activate", new Dictionary<string, object>());
        }

        public Task<Dictionary<string, object>> PauseAsync(string sequenceId)
        {
            return _http.PostAsync($"/sequences/{sequenceId}/pause", new Dictionary<string, object>());
        }
    }
}
