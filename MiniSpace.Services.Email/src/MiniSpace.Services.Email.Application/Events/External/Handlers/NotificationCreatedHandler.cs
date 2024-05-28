    using System;
    using System.Threading.Tasks;
    using Convey.CQRS.Events;
    using Convey.MessageBrokers;
    using MiniSpace.Services.Email.Core.Repositories;
    using MiniSpace.Services.Email.Core.Entities;
    using MiniSpace.Services.Email.Application.Services;
    using System.Text.Json.Serialization;
    using System.Text.Json;


    namespace MiniSpace.Services.Email.Application.Events.External.Handlers
    {
        public class NotificationCreatedHandler : IEventHandler<NotificationCreated>
        {
            private readonly IStudentEmailsRepository _studentEmailsRepository;
            private readonly IEmailService _emailService;
            private readonly IMessageBroker _messageBroker;

            public NotificationCreatedHandler(
                IStudentEmailsRepository studentEmailsRepository, 
                IMessageBroker messageBroker,
                IEmailService emailService) 
            {
                _studentEmailsRepository = studentEmailsRepository;
                _messageBroker = messageBroker;
                _emailService = emailService;
            }
            public async Task HandleAsync(NotificationCreated @event, CancellationToken cancellationToken)
            {
                Console.WriteLine("*********************************************************************");
                    string jsonEvent = JsonSerializer.Serialize(@event); // Correct use of JsonSerializer
                Console.WriteLine($"Received Event: {jsonEvent}");

                var studentEmails = await _studentEmailsRepository.GetByStudentIdAsync(@event.UserId);
                if (studentEmails == null)
                {
                    // return;
                }

                // var userPrefersEmails = studentEmails.ShouldReceiveEmailNotifications();
                // if (!userPrefersEmails)
                // {
                //     return;
                // }

                var emailNotification = new EmailNotification(
                    Guid.NewGuid(),
                    @event.UserId,
                    // studentEmails.EmailAddress,
                    "voznesenskijandrej5@gmail.com", 
                    "Notification: " + @event.Message,
                    "You have a new notification created at: " + @event.CreatedAt.ToString("g"),
                    EmailNotificationStatus.Pending
                );
                
                var emailAddress = "voznesenskijandrej5@gmail.com"; // or a default for testing

                var subject = "Notification: " + @event.Message;
                var body = "You have a new notification created at: " + @event.CreatedAt.ToString("g");

                // Send the email
                await _emailService.SendEmailAsync(emailAddress, subject, body);
                Console.WriteLine($"Email sent to {emailAddress}");

                // studentEmails.AddEmailNotification(emailNotification);
                // await _studentEmailsRepository.UpdateAsync(studentEmails);

                await _messageBroker.PublishAsync(new EmailQueued(emailNotification.EmailNotificationId, @event.UserId));
            }
        }
    }
