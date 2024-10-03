using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace Astravent.Web.Wasm.Utilities
{
    public class IPAddressService : IIPAddressService
    {
        private readonly HttpClient _httpClient;

        public IPAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetClientIpAddressAsync()
        {
            try
            {
                Console.WriteLine("Getting IP address...");
                var response = await _httpClient.GetStringAsync("https://api.ipify.org?format=json");

                var ipResponse = JsonSerializer.Deserialize<IpResponse>(response);
                Console.WriteLine($"IP address: {ipResponse?.Ip ?? "Unknown"}");

                return ipResponse?.Ip ?? "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        private class IpResponse
        {
            public string Ip { get; set; }
        }
    }
}
