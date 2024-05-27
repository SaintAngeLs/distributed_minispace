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
                NotificationNotFoundException ex => message switch
                {
                    DeleteNotification command => new NotificationDeletionRejected(command.NotificationId, "Notification not found", ex.Code),
                    UpdateNotificationStatus command => new NotificationUpdateRejected(command.NotificationId, "Notification not found", ex.Code),
                    CreateNotification command => new NotificationCreationRejected(command.NotificationId, "Notification not found", ex.Code),
                    _ => new NotificationProcessRejected(ex.NotificationId, ex.Message, ex.Code),
                },
                InvalidNotificationStatusException ex => message switch
                {
                    UpdateNotificationStatus command => new NotificationUpdateRejected(command.NotificationId, ex.Message, ex.Code),
                    _ => new NotificationProcessRejected(Guid.Empty, ex.Message, ex.Code),
                },
                NotificationAlreadyDeletedException ex => message switch
                {
                    DeleteNotification command => new NotificationDeletionRejected(command.NotificationId, ex.Message, ex.Code),
                    UpdateNotificationStatus command => new NotificationUpdateRejected(command.NotificationId, ex.Message, ex.Code),
                    _ => new NotificationProcessRejected(ex.NotificationId, ex.Message, ex.Code),
                },
                AppException ex => message switch
                {
                    _ => new NotificationProcessRejected(Guid.Empty, ex.Message, ex.Code)
                },
                _ => null 
            };
    }
}
