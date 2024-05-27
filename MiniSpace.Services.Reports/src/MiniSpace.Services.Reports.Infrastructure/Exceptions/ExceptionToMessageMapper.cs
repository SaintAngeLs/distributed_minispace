using Convey.MessageBrokers.RabbitMQ;
// using MiniSpace.Services.Reports.Application.Commands;
// using MiniSpace.Services.Reports.Application.Events.Rejected;
// using MiniSpace.Services.Reports.Application.Events.External;
// using MiniSpace.Services.Reports.Application.Exceptions;
// using MiniSpace.Services.Reports.Core;

namespace MiniSpace.Services.Reports.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
    
            {
                _ => null
            };
    }    
}
