using Convey.MessageBrokers.RabbitMQ;
// using MiniSpace.Services.Organizations.Application.Commands;
// using MiniSpace.Services.Organizations.Application.Events.Rejected;
// using MiniSpace.Services.Organizations.Application.Events.External;
// using MiniSpace.Services.Organizations.Application.Exceptions;
// using MiniSpace.Services.Organizations.Core;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
    
            {
                _ => null
            };
    }    
}
