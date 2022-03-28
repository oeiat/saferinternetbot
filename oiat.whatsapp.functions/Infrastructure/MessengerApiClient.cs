using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace oiat.whatsapp.functions.Infrastructure
{
    public class MessengerApiClient : IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly string _clientId;
        protected readonly string _clientSecret;
        protected readonly string _scope;
        protected readonly string _authUrl;
        protected readonly string _sendUrl;

        public string AccessToken { get; protected set; }

        public MessengerApiClient(string clientId, string clientSecret, string scope, string authUrl = "https://auth.messengerpeople.dev/token", string sendUrl = "https://api.messengerpeople.dev/messages")
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _scope = scope;

            _client = new HttpClient();
            _authUrl = authUrl;
            _sendUrl = sendUrl;
        }

        public virtual async Task Authenticate()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "grant_type","client_credentials" },
                { "client_id",_clientId },
                { "client_secret",_clientSecret },
                { "scope", _scope }
            });

            var result = await _client.PostAsync(_authUrl, content);

            if (result.IsSuccessStatusCode)
            {
                var resultContent = await result.Content.ReadAsStringAsync();
                dynamic resultObject = JsonConvert.DeserializeObject(resultContent);
                if (resultObject.token_type != null && resultObject.token_type == "Bearer")
                {
                    AccessToken = resultObject.access_token;
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                }
            }
        }

        public virtual async Task<HttpResponseMessage> PostMessage(string uri, HttpContent content)
        {
            await Authenticate();
            return await _client.PostAsync(uri, content);
        }

        public virtual async Task<string> SendMessage(string message)
        {
            var content = new StringContent(message, Encoding.UTF8, "application/json");
            var result = await PostMessage(_sendUrl, content);

            var jsonResult = await result.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonResult);

            return data.id;
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
