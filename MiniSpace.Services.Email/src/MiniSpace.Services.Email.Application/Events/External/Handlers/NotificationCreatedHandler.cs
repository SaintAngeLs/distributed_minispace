using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MiniSpace.Services.Email.Core.Repositories;
using MiniSpace.Services.Email.Core.Entities;
using MiniSpace.Services.Email.Application.Services;

namespace MiniSpace.Services.Email.Application.Events.External.Handlers
{
    public class NotificationCreatedHandler : IEventHandler<NotificationCreated>
    {
        private readonly IStudentEmailsRepository _studentEmailsRepository;
        private readonly IMessageBroker _messageBroker;

        public NotificationCreatedHandler(IStudentEmailsRepository studentEmailsRepository, IMessageBroker messageBroker)
        {
            _studentEmailsRepository = studentEmailsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(NotificationCreated @event, CancellationToken cancellationToken)
        {
            var studentEmails = await _studentEmailsRepository.GetByStudentIdAsync(@event.UserId);
            if (studentEmails == null)
            {
                return;
            }

            var userPrefersEmails = studentEmails.ShouldReceiveEmailNotifications();
            if (!userPrefersEmails)
            {
                return;
            }

            var emailNotification = new EmailNotification(
                Guid.NewGuid(),
                @event.UserId,
                studentEmails.EmailAddress, 
                "Notification: " + @event.Message,
                "You have a new notification created at: " + @event.CreatedAt.ToString("g"),
                EmailNotificationStatus.Pending
            );

            studentEmails.AddEmailNotification(emailNotification);
            await _studentEmailsRepository.UpdateAsync(studentEmails);

            await _messageBroker.PublishAsync(new EmailQueued(emailNotification.EmailNotificationId, @event.UserId));
        }
    }
}
