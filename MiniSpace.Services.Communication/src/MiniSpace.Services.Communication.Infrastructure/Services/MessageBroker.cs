using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTracing;
using MiniSpace.Services.Communication.Application.Services;
using MiniSpace.Services.Communication.Infrastructure;
using System.Text.Json;

namespace MiniSpace.Services.Communication.Infrastructure.Services
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
                  var serializedEvent = JsonSerializer.Serialize(@event, new JsonSerializerOptions
                {
                    WriteIndented = true, // To make it easier to read in logs
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
             
                _logger.LogTrace($"Publishing integration event: {@event.GetType().Name} [id: '{messageId}'], Content: {serializedEvent}");
                // Console.WriteLine($"Publishing Event: {@event.GetType().Name} with ID: {messageId}, Content: {serializedEvent}");
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
