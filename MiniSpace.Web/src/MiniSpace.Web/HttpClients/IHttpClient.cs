using System.Threading.Tasks;
using MiniSpace.Web.Areas.Friends;

namespace MiniSpace.Web.HttpClients
{
    public interface IHttpClient
    {
        void SetAccessToken(string accessToken);
        Task<T> GetAsync<T>(string uri);
        Task PostAsync<T>(string uri, T request);
        Task<HttpResponse<TResult>> PostAsync<TRequest, TResult>(string uri, TRequest request);
        Task PutAsync<T>(string uri, T request);
        Task<HttpResponse<TResult>> PutAsync<TRequest, TResult>(string uri, TRequest request);
        Task DeleteAsync(string uri);
        Task DeleteAsync(string uri, object payload);
    }
}