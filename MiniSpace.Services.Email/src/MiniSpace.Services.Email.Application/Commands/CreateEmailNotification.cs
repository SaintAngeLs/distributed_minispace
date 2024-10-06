using Paralax.CQRS.Commands;
using System;

namespace MiniSpace.Services.Email.Application.Commands
{
    public class CreateEmailNotification : ICommand
    {
        public Guid EmailNotificationId { get; private set; }
        public Guid UserId { get; }
        public string EmailAddress { get; }
        public string Subject { get; }
        public string Body { get; }

        public CreateEmailNotification(Guid userId, string emailAddress, string subject, string body)
        {
            EmailNotificationId = Guid.NewGuid(); 
            UserId = userId;
            EmailAddress = emailAddress;
            Subject = subject;
            Body = body;
        }
    }
}
