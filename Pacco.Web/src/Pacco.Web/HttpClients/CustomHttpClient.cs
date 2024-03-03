using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;

namespace Pacco.Web.HttpClients
{
    public class CustomHttpClient : IHttpClient
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true
        };
        
        private readonly HttpClient _client;
        private readonly HttpClientOptions _options;
        private readonly ILogger<IHttpClient> _logger;
        
        public CustomHttpClient(HttpClient client, HttpClientOptions options, ILogger<IHttpClient> logger)
        {
            _client = client;
            _options = options;
            _logger = logger;
            _client.BaseAddress = new Uri(options.ApiUrl);
        }
        
        public void SetAccessToken(string accessToken)
            => _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        public async Task<T> GetAsync<T>(string uri)
        {
            var (success, content) = await TryExecuteAsync(uri, client => client.GetAsync(uri));

            return !success ? default : Parse<T>(content);
        }

        public Task PostAsync<T>(string uri, T request)
            => TryExecuteAsync(uri, client => client.PostAsync(uri, GetPayload(request)));

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest request)
        {
            var (success, content) = await TryExecuteAsync(uri, client => client.PostAsync(uri, GetPayload(request)));

            return !success ? default : Parse<TResult>(content);
        }

        public Task PutAsync<T>(string uri, T request)
            => TryExecuteAsync(uri, client => client.PutAsync(uri, GetPayload(request)));

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest request)
        {
            var (success, content) = await TryExecuteAsync(uri, client => client.PutAsync(uri, GetPayload(request)));

            return !success ? default : Parse<TResult>(content);
        }

        public Task DeleteAsync(string uri)
            => TryExecuteAsync(uri, client => client.DeleteAsync(uri));
        
        private static StringContent GetPayload<T>(T request)
            => new StringContent(JsonSerializer.Serialize(request, JsonSerializerOptions), Encoding.UTF8,
                "application/json");
        
        private Task<(bool success, string content)> TryExecuteAsync(string uri,
            Func<HttpClient, Task<HttpResponseMessage>> client)
            => Policy.Handle<Exception>()
                .WaitAndRetryAsync(_options.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
                .ExecuteAsync(async () =>
                {
                    uri = uri.StartsWith("http://") ? uri : $"http://{uri}";
                    _logger.LogDebug($"Sending HTTP request to URI: {uri}");
                    using (var response = await client(_client))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            _logger.LogDebug($"Received a valid response to HTTP request from URI: {uri}" +
                                             $"{Environment.NewLine}{response}");

                            return (true, await response.Content.ReadAsStringAsync());
                        }

                        _logger.LogError($"Received an invalid response to HTTP request from URI: {uri}" +
                                         $"{Environment.NewLine}{response}");

                        return default;
                    }
                });
        
        private static T Parse<T>(string content)
            => string.IsNullOrWhiteSpace(content) ? default : JsonSerializer.Deserialize<T>(content, JsonSerializerOptions);
    }
}