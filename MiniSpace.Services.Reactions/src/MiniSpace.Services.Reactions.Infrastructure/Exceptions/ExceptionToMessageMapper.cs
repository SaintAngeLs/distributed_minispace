using System.Diagnostics.CodeAnalysis;
using Paralax.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Reactions.Application.Commands;
using MiniSpace.Services.Reactions.Application.Events.Rejected;
using MiniSpace.Services.Reactions.Application.Exceptions;
using MiniSpace.Services.Reactions.Core.Exceptions;

namespace MiniSpace.Services.Reactions.Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                InvalidReactionTypeException ex => new AddReactionRejected(Guid.Empty, ex.Message, ex.Code),
                InvalidAggregateIdException ex => new AddReactionRejected(Guid.Empty, ex.Message, ex.Code),
                InvalidReactionContentTypeException ex => new AddReactionRejected(Guid.Empty, ex.Message, ex.Code),
                EventNotFoundException ex => new AddReactionRejected(Guid.Empty, ex.Message, ex.Code),
                _ => null,
            };
    }    
}