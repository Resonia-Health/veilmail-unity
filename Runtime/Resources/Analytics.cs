using System.Collections.Generic;
using System.Threading.Tasks;
using VeilMail.Http;

namespace VeilMail.Resources
{
    public class Analytics
    {
        private readonly VeilMailHttpClient _http;

        internal Analytics(VeilMailHttpClient http) { _http = http; }

        public Task<Dictionary<string, object>> GetOverviewAsync(string startDate = null, string endDate = null)
        {
            var query = "/analytics/overview";
            var queryParams = new List<string>();
            if (startDate != null) queryParams.Add($"startDate={startDate}");
            if (endDate != null) queryParams.Add($"endDate={endDate}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetEmailStatsAsync(string startDate = null, string endDate = null)
        {
            var query = "/analytics/emails";
            var queryParams = new List<string>();
            if (startDate != null) queryParams.Add($"startDate={startDate}");
            if (endDate != null) queryParams.Add($"endDate={endDate}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }

        public Task<Dictionary<string, object>> GetCampaignStatsAsync(string campaignId = null, string startDate = null, string endDate = null)
        {
            var query = "/analytics/campaigns";
            var queryParams = new List<string>();
            if (campaignId != null) queryParams.Add($"campaignId={campaignId}");
            if (startDate != null) queryParams.Add($"startDate={startDate}");
            if (endDate != null) queryParams.Add($"endDate={endDate}");
            if (queryParams.Count > 0) query += "?" + string.Join("&", queryParams);
            return _http.GetAsync(query);
        }
    }
}
