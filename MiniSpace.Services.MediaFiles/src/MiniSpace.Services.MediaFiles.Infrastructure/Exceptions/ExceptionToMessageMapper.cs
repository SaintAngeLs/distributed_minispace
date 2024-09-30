using Paralax.MessageBrokers.RabbitMQ;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Exceptions
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
