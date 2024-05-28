using System;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using MiniSpace.Services.Email.Core.Entities;
using MiniSpace.Services.Email.Application.Services;
using MiniSpace.Services.Email.Application.Services.Clients;
using System.Text.Json;
using MiniSpace.Services.Email.Application.Exceptions;
using MiniSpace.Services.Email.Core.Repositories;

namespace MiniSpace.Services.Email.Application.Events.External.Handlers
{
    public class NotificationCreatedHandler : IEventHandler<NotificationCreated>
    {
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEmailService _emailService;
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentEmailsRepository _studentEmailsRepository;

        public NotificationCreatedHandler(
            IStudentsServiceClient studentsServiceClient, 
            IMessageBroker messageBroker,
            IEmailService emailService,
            IStudentEmailsRepository studentEmailsRepository) 
        {
            _studentsServiceClient = studentsServiceClient;
            _messageBroker = messageBroker;
            _emailService = emailService;
            _studentEmailsRepository = studentEmailsRepository;
        }

        public async Task HandleAsync(NotificationCreated @event, CancellationToken cancellationToken)
        {
            Console.WriteLine("*********************************************************************");
            string jsonEvent = JsonSerializer.Serialize(@event);
            Console.WriteLine($"Received Event: {jsonEvent}");

            var student = await _studentsServiceClient.GetAsync(@event.UserId);
            if (student == null)
            {
                throw new EmailNotFoundException(@event.UserId);
            }

            if (!student.EmailNotifications)
            {
                throw new EmailNotificationDisabledException(@event.UserId);
            }

            string htmlContent = await LoadHtmlTemplate("email_template1.html", @event);
        
            var subject = EmailSubjectFactory.CreateSubject((NotificationEventType)Enum.Parse(typeof(NotificationEventType), @event.EventType), @event.Details);

            var emailNotification = new EmailNotification(
                Guid.NewGuid(),
                @event.UserId,
                student.Email, 
                subject,
                htmlContent, 
                EmailNotificationStatus.Pending
            );

            // Send the email
            await _emailService.SendEmailAsync(student.Email, subject, htmlContent);
            Console.WriteLine($"Email sent to {student.Email}");

            // Add or update the student emails document with the new notification
            var studentEmails = await _studentEmailsRepository.GetByStudentIdAsync(@event.UserId) ?? new StudentEmails(@event.UserId);
            studentEmails.AddEmailNotification(emailNotification);
            await _studentEmailsRepository.UpdateAsync(studentEmails);

            // Publish that the email has been queued
            await _messageBroker.PublishAsync(new EmailQueued(emailNotification.EmailNotificationId, @event.UserId));
        }

        private async Task<string> LoadHtmlTemplate(string filePath, NotificationCreated eventDetails)
        {
            string htmlContent = await System.IO.File.ReadAllTextAsync(filePath);
            htmlContent = htmlContent.Replace("{Message}", eventDetails.Message);
            htmlContent = htmlContent.Replace("{Details}", eventDetails.Details ?? "No details provided");
            htmlContent = htmlContent.Replace("{CreatedAt}", eventDetails.CreatedAt.ToString("dddd, dd MMMM yyyy"));
            htmlContent = htmlContent.Replace("{EventType}", eventDetails.EventType);
            htmlContent = htmlContent.Replace("{UserName}", "User"); 

            return htmlContent;
        }

    }
}
