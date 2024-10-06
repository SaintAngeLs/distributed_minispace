using Paralax.CQRS.Events;
using Paralax.MessageBrokers;
using Paralax.MessageBrokers.Outbox;
using Paralax.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTracing;
using MiniSpace.Services.Comments.Application.Services;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MiniSpace.Services.Comments.Infrastructure.UnitTests")]
namespace MiniSpace.Services.Comments.Infrastructure.Services
{
    internal sealed class MessageBroker : IMessageBroker
    {
        private const string DefaultSpanContextHeader = "span_context";
        private readonly IBusPublisher _busPublisher;
        private readonly IMessageOutbox _outbox;
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly ITracer _tracer;
        private readonly ILogger<IMessageBroker> _logger;
        private readonly string _spanContextHeader;

        public MessageBroker(IBusPublisher busPublisher, IMessageOutbox outbox,
            ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor,
            IMessagePropertiesAccessor messagePropertiesAccessor, RabbitMqOptions options, ITracer tracer,
            ILogger<IMessageBroker> logger)
        {
            _busPublisher = busPublisher;
            _outbox = outbox;
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _messagePropertiesAccessor = messagePropertiesAccessor;
            _tracer = tracer;
            _logger = logger;
            _spanContextHeader = string.IsNullOrWhiteSpace(options.SpanContextHeader)
                ? DefaultSpanContextHeader
                : options.SpanContextHeader;
        }

        public Task PublishAsync(params IEvent[] events) => PublishAsync(events?.AsEnumerable());

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            if (events is null)
            {
                return;
            }

            var messageProperties = _messagePropertiesAccessor.MessageProperties;
            var originatedMessageId = messageProperties?.MessageId;
            var correlationId = messageProperties?.CorrelationId;
            var spanContext = messageProperties?.GetSpanContext(_spanContextHeader);
            if (string.IsNullOrWhiteSpace(spanContext))
            {
                spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();
            }

            var headers = messageProperties.GetHeadersToForward();
            var correlationContext = _contextAccessor.CorrelationContext ??
                                     _httpContextAccessor.GetCorrelationContext();

            foreach (var @event in events)
            {
                if (@event is null)
                {
                    continue;
                }

                var messageId = Guid.NewGuid().ToString("N");
                _logger.LogTrace($"Publishing integration event: {@event.GetType().Name} [id: '{messageId}'].");
                if (_outbox.Enabled)
                {
                    await _outbox.SendAsync(@event, originatedMessageId, messageId, correlationId, spanContext,
                        correlationContext, headers);
                    continue;
                }

                await _busPublisher.PublishAsync(@event, messageId, correlationId, spanContext, correlationContext,
                    headers);
            }
        }
    }
}
