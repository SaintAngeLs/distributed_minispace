using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OpenTracing;

namespace MiniSpace.APIGateway.Ocelot.Infrastructure
{
    internal sealed class CorrelationContextHandler : DelegatingHandler
    {
        private static readonly ISet<string> OperationHeaderRequests = new HashSet<string>
        {
            "POST", "PUT", "PATCH", "DELETE"
        };

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICorrelationContextBuilder _correlationContextBuilder;
        private readonly ITracer _tracer;

        public CorrelationContextHandler(IHttpContextAccessor httpContextAccessor,
            ICorrelationContextBuilder correlationContextBuilder, ITracer tracer)
        {
            _httpContextAccessor = httpContextAccessor;
            _correlationContextBuilder = correlationContextBuilder;
            _tracer = tracer;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();
            var correlationId = Guid.NewGuid().ToString("N");
            var resourceId = httpContext.GetResourceIdFoRequest();
            var correlationContext = _correlationContextBuilder.Build(httpContext, correlationId, spanContext,
                resourceId: resourceId);
            request.Headers.TryAddWithoutValidation("Correlation-Context",
                JsonConvert.SerializeObject(correlationContext));

            if (OperationHeaderRequests.Contains(httpContext.Request.Method))
            {
                httpContext.Response.SetOperationHeader(correlationId);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}