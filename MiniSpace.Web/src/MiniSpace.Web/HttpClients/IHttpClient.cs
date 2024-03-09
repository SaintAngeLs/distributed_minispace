using System.Threading.Tasks;

namespace MiniSpace.Web.HttpClients
{
    public interface IHttpClient
    {
        void SetAccessToken(string accessToken);
        Task<T> GetAsync<T>(string uri);
        Task PostAsync<T>(string uri, T request);
        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest request);
        Task PutAsync<T>(string uri, T request);
        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest request);
        Task DeleteAsync(string uri);
    }
}