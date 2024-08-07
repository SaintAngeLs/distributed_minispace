using Microsoft.AspNetCore.Http;

namespace MiniSpace.APIGateway.Ocelot.Infrastructure
{
    internal interface ICorrelationContextBuilder
    {
        CorrelationContext Build(HttpContext context, string correlationId, string spanContext, string name = null,
            string resourceId = null);
    }
}