using Convey.CQRS.Commands;

namespace MiniSpace.Services.Email.Application.Commands
{
    public class CreateNotification : ICommand
    {
        public Guid NotificationId { get; }
        public Guid UserId { get; }
        public string Message { get; }

        public CreateNotification(Guid notificationId, Guid userId, string message)
        {
            NotificationId = notificationId;
            UserId = userId;
            Message = message;
        }
    }
}
