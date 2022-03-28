using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using oiat.saferinternetbot.LuisApi.Models;

namespace oiat.saferinternetbot.LuisApi.ApiClient
{
    public class RestLuisApiClient : ILuisApiClient
    {
        private readonly string _path;
        private readonly string _pathScore;
        private readonly string _key;

        public RestLuisApiClient(string baseUrl, string baseScoreUrl, string appId, string appVersion, string key, string scoreKey)
        {
            _key = key;
            _path = baseUrl + appId + "/versions/" + appVersion + "/";
            _pathScore = baseScoreUrl + appId + "/slots/production/predict?subscription-key=" + scoreKey + "&verbose=true&timezoneOffset=0&log=true&query=";
        }

        public async Task<ScoreApiModel> GetScore(string query)
        {
            if (query.Length > 500)
            {
                query = query.Substring(0, 500);
            }

            var response = await Get(_pathScore + Uri.EscapeDataString(query), false);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ScoreApiModel>(result);
        }

        public async Task<IEnumerable<IntentApiModel>> GetAllIntents()
        {
            var response = await Get(_path + "intents");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<IntentApiModel>>(result);
        }

        private async Task<HttpResponseMessage> Get(string url, bool appendKey = true)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri(url);

                    if (appendKey)
                    {
                        request.Headers.Add("Ocp-Apim-Subscription-Key", _key);
                    }
                    return await client.SendAsync(request);
                }
            }
        }


    }
}
