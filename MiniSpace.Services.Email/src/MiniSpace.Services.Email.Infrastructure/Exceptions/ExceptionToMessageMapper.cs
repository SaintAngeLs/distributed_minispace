using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Email.Application.Commands;
using MiniSpace.Services.Email.Application.Events.Rejected;
using MiniSpace.Services.Email.Application.Exceptions;

namespace MiniSpace.Services.Email.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                EmailNotFoundException ex => message switch
                {
                    CreateEmailNotification command => new EmailCreationRejected(command.EmailNotificationId, "Email not found", ex.Code),
                    _ => new EmailSendingRejected(ex.EmailNotificationId, "Email not found", ex.Code),
                },
                InvalidEmailStatusException ex => new EmailQueueingRejected(Guid.Empty, ex.Message, ex.Code),
                EmailAlreadySentException ex => message switch
                {
                    CreateEmailNotification command => new EmailCreationRejected(command.EmailNotificationId, "Email already sent", ex.Code),
                    _ => new EmailSendingRejected(ex.EmailNotificationId, "Email already sent", ex.Code),
                },
                AppException ex => new EmailSendingRejected(Guid.Empty, ex.Message, ex.Code),
                _ => null 
            };
    }
}
