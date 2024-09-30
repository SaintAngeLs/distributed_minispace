using Paralax.CQRS.Commands;
using Paralax.MessageBrokers;
using Paralax.MessageBrokers.Outbox;
using Paralax.Types;

namespace MiniSpace.Services.Email.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class OutboxCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly IMessageOutbox _outbox;
        private readonly string _messageId;
        private readonly bool _enabled;

        public OutboxCommandHandlerDecorator(ICommandHandler<TCommand> handler, IMessageOutbox outbox,
            OutboxOptions outboxOptions, IMessagePropertiesAccessor messagePropertiesAccessor)
        {
            _handler = handler;
            _outbox = outbox;
            _enabled = outboxOptions.Enabled;

            var messageProperties = messagePropertiesAccessor.MessageProperties;
            _messageId = string.IsNullOrWhiteSpace(messageProperties?.MessageId)
                ? Guid.NewGuid().ToString("N")
                : messageProperties.MessageId;
        }

        public Task HandleAsync(TCommand command, CancellationToken cancellationToken)
            => _enabled
                ? _outbox.HandleAsync(_messageId, () => _handler.HandleAsync(command))
                : _handler.HandleAsync(command);
    }
}
