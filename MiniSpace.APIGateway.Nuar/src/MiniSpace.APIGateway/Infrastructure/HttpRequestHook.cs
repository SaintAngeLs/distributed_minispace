using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nuar;
using Nuar.RabbitMq;
using Nuar.Hooks;

namespace MiniSpace.APIGateway.Infrastructure
{
    internal sealed class HttpRequestHook : IHttpRequestHook
    {
        private readonly IContextBuilder _contextBuilder;

        public HttpRequestHook(IContextBuilder contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }


        public Task InvokeAsync(HttpRequestMessage request, ExecutionData data)
        {
            var context = JsonConvert.SerializeObject(_contextBuilder.Build(data));
            request.Headers.TryAddWithoutValidation("Correlation-Context", context);
            
            return Task.CompletedTask;
        }
    }
}