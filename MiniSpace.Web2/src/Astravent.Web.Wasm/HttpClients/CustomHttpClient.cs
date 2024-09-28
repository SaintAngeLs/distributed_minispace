using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using NetJSON;

namespace Astravent.Web.Wasm.HttpClients
{
    public class CustomHttpClient : IHttpClient
    {
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

        public void SetAccessToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var (success, content) = await TryExecuteAsync(uri, client => client.GetAsync(uri));
            if (!success)
                return default;

            return NetJSON.NetJSON.Deserialize<T>(content);
        }

        public Task PostAsync<T>(string uri, T request)
        {
            var jsonPayload = NetJSON.NetJSON.Serialize(request);
            _logger.LogDebug($"Sending HTTP POST request to URI: {uri} with payload: {jsonPayload}");
            return TryExecuteAsync(uri, client => client.PostAsync(uri, GetPayload(request)));
        }

        public async Task<HttpResponse<TResult>> PostAsync<TRequest, TResult>(string uri, TRequest request)
        {
            var jsonPayload = NetJSON.NetJSON.Serialize(request);
            _logger.LogDebug($"Sending HTTP POST request to URI: {uri} with payload: {jsonPayload}");

            var (success, content) = await TryExecuteAsync(uri, client => client.PostAsync(uri, GetPayload(request)));
            if (!success)
                return new HttpResponse<TResult> { ErrorMessage = NetJSON.NetJSON.Deserialize<ErrorMessage>(content) };

            return new HttpResponse<TResult> { Content = NetJSON.NetJSON.Deserialize<TResult>(content) };
        }

        public Task PutAsync<T>(string uri, T request)
            => TryExecuteAsync(uri, client => client.PutAsync(uri, GetPayload(request)));

        public async Task<HttpResponse<TResult>> PutAsync<TRequest, TResult>(string uri, TRequest request)
        {
            var (success, content) = await TryExecuteAsync(uri, client => client.PutAsync(uri, GetPayload(request)));
            if (!success)
                return new HttpResponse<TResult> { ErrorMessage = NetJSON.NetJSON.Deserialize<ErrorMessage>(content) };

            return new HttpResponse<TResult> { Content = NetJSON.NetJSON.Deserialize<TResult>(content) };
        }

        public Task DeleteAsync(string uri)
            => TryExecuteAsync(uri, client => client.DeleteAsync(uri));

        public async Task DeleteAsync(string uri, object payload)
        {
            var jsonPayload = NetJSON.NetJSON.Serialize(payload);
            _logger.LogInformation($"Sending HTTP DELETE request to URI: {uri} with payload: {jsonPayload}");

            var request = new HttpRequestMessage(HttpMethod.Delete, uri)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Error response from server: {errorContent}");
                throw new HttpRequestException($"Request to {uri} failed with status code {response.StatusCode} and message {errorContent}");
            }
        }

        private static StringContent GetPayload<T>(T request)
        {
            var json = NetJSON.NetJSON.Serialize(request);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<(bool success, string content)> TryExecuteAsync(string uri, Func<HttpClient, Task<HttpResponseMessage>> client)
            => await Policy.Handle<Exception>()
                .WaitAndRetryAsync(_options.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
                .ExecuteAsync(async () =>
                {
                    if (_client.BaseAddress != null && !Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                    {
                        if (!uri.StartsWith("/")) uri = "/" + uri;
                        uri = new Uri(_client.BaseAddress, uri).ToString();
                    }
                    else if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                    {
                        _logger.LogError($"The provided URI '{uri}' is not a valid absolute URL and no BaseAddress is set.");
                        return default;
                    }

                    _logger.LogDebug($"Sending HTTP request to URI: {uri}");
                    using (var response = await client(_client))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            _logger.LogDebug($"Received a valid response to HTTP request from URI: {uri}" +
                                             $"{Environment.NewLine}{response}");

                            return (true, await response.Content.ReadAsStringAsync());
                        }

                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Error response from server: {errorContent}");
                        return (false, errorContent);
                    }
                });
    }
}
