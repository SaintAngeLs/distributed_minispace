using System;
using Microsoft.Extensions.DependencyInjection;
using Nuar;
using Nuar.RabbitMq;
using OpenTracing;

namespace MiniSpace.APIGateway.Infrastructure
{
    internal sealed class SpanContextBuilder : ISpanContextBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public SpanContextBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string Build(ExecutionData executionData)
        {
            var tracer = _serviceProvider.GetService<ITracer>();
            var spanContext = tracer is null ? string.Empty :
                tracer.ActiveSpan is null ? string.Empty : tracer.ActiveSpan.Context.ToString();

            return spanContext;
        }
    }
}