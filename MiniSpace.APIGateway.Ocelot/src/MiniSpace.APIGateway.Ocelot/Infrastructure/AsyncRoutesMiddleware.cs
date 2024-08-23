using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.MessageBrokers.RabbitMQ;
using Convey.MessageBrokers.RabbitMQ.Conventions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OpenTracing;


namespace MiniSpace.APIGateway.Ocelot.Infrastructure
{
    internal sealed class AsyncRoutesMiddleware : IMiddleware
    {
        private static readonly ConcurrentDictionary<string, IConventions> Conventions =
            new ConcurrentDictionary<string, IConventions>();

        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly IPayloadBuilder _payloadBuilder;
        private readonly ITracer _tracer;
        private readonly ICorrelationContextBuilder _correlationContextBuilder;
        private readonly IAnonymousRouteValidator _anonymousRouteValidator;
        private readonly IDictionary<string, RouteOptions> _routes;
        private readonly bool _authenticate;

        public AsyncRoutesMiddleware(IRabbitMqClient rabbitMqClient, IPayloadBuilder payloadBuilder, ITracer tracer,
            ICorrelationContextBuilder correlationContextBuilder, IAnonymousRouteValidator anonymousRouteValidator,
            IOptions<AsyncRoutesOptions> asyncRoutesOptions)
        {
            _rabbitMqClient = rabbitMqClient;
            _payloadBuilder = payloadBuilder;
            _tracer = tracer;
            _correlationContextBuilder = correlationContextBuilder;
            _anonymousRouteValidator = anonymousRouteValidator;
            _routes = asyncRoutesOptions.Value.Routes;
            _authenticate = asyncRoutesOptions.Value.Authenticate == true;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_routes is null || !_routes.Any())
            {
                await next(context);
                return;
            }

            var key = GetKey(context);
            if (!_routes.TryGetValue(key, out var route))
            {
                await next(context);
                return;
            }

            if ((_authenticate && route.Authenticate != false || route.Authenticate == true) &&
                !_anonymousRouteValidator.HasAccess(context.Request.Path))
            {
                var authenticateResult = await context.AuthenticateAsync();
                if (!authenticateResult.Succeeded)
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                context.User = authenticateResult.Principal;
            }

            if (!Conventions.TryGetValue(key, out var conventions))
            {
                conventions = new MessageConventions(typeof(object), route.RoutingKey, route.Exchange, null);
                Conventions.TryAdd(key, conventions);
            }

            var spanContext = _tracer.ActiveSpan?.Context.ToString() ?? string.Empty;

            var message = await _payloadBuilder.BuildFromJsonAsync<object>(context.Request);
            var resourceId = Guid.NewGuid().ToString("N");
            if (context.Request.Method == "POST" && message is JObject jObject)
            {
                jObject.SetResourceId(resourceId);
            }

            var messageId = Guid.NewGuid().ToString("N");
            var correlationId = Guid.NewGuid().ToString("N");
            var correlationContext = _correlationContextBuilder.Build(context, correlationId, spanContext,
                route.RoutingKey, resourceId);
            _rabbitMqClient.Send(message, conventions, messageId, correlationId, spanContext, correlationContext);
            context.Response.StatusCode = 202;
            context.Response.SetOperationHeader(correlationId);
        }

        private static string GetKey(HttpContext context) => $"{context.Request.Method} {context.Request.Path}";
    }
}