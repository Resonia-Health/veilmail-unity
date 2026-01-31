using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Emails
    {
        private readonly VeilMailHttpClient _http;

        internal Emails(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> SendAsync(Dictionary<string, object> parameters)
        {
            return _http.PostAsync("/emails", parameters);
        }

        public Task<Dictionary<string, object>> GetAsync(string emailId)
        {
            return _http.GetAsync($"/emails/{emailId}");
        }

        public Task<Dictionary<string, object>> ListAsync(string cursor = null, int? limit = null)
        {
            var query = "/emails";
            var queryParams = new List<string>();
            if (cursor != null) queryParams.Add($"cursor={cursor}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> CancelAsync(string emailId)
        {
            return _http.PostAsync($"/emails/{emailId}/cancel", new Dictionary<string, object>());
        }
    }
}
