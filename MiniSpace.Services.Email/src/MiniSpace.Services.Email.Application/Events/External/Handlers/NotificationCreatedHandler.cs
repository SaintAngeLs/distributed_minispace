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
using MiniSpace.Services.Email.Application.Dto;
using Microsoft.Extensions.Logging;

namespace MiniSpace.Services.Email.Application.Events.External.Handlers
{
    public class NotificationCreatedHandler : IEventHandler<NotificationCreated>
    {
        private readonly IStudentsServiceClient _studentsServiceClient;
        private readonly IEmailService _emailService;
        private readonly IMessageBroker _messageBroker;
        private readonly IStudentEmailsRepository _studentEmailsRepository;
        private readonly ILogger<NotificationCreatedHandler> _logger; 

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
            string jsonEvent = JsonSerializer.Serialize(@event);
            _logger.LogInformation($"Received Event: {jsonEvent}");


            var student = await _studentsServiceClient.GetAsync(@event.UserId);
            if (student == null)
            {
                throw new EmailNotFoundException(@event.UserId);
            }

            if (!student.EmailNotifications && student.State == "Valid")
            {
                throw new EmailNotificationDisabledException(@event.UserId);
            }

            string htmlContent = await LoadHtmlTemplate("new_message.html", @event, student);

            var subject = EmailSubjectFactory.CreateSubject((NotificationEventType)Enum.Parse(typeof(NotificationEventType), @event.EventType), @event.Details);

            var emailNotification = new EmailNotification(
                Guid.NewGuid(),
                @event.UserId,
                student.Email, 
                subject,
                htmlContent, 
                EmailNotificationStatus.Pending
            );
            await _emailService.SendEmailAsync(student.Email, subject, htmlContent);
            _logger.LogInformation($"Email sent to {student.Email}");

            emailNotification.MarkAsSent();

            var studentEmails = await _studentEmailsRepository.GetByStudentIdAsync(@event.UserId) ?? new StudentEmails(@event.UserId);
            studentEmails.AddEmailNotification(emailNotification);
            await _studentEmailsRepository.UpdateAsync(studentEmails);
            
            await _messageBroker.PublishAsync(new EmailQueued(emailNotification.EmailNotificationId, @event.UserId));
        }


        private async Task<string> LoadHtmlTemplate(string filePath, NotificationCreated eventDetails, StudentDto student)
        {
            string htmlContent = await System.IO.File.ReadAllTextAsync(filePath);
            htmlContent = htmlContent.Replace("{Message}", eventDetails.Message);
            htmlContent = htmlContent.Replace("{Details}", eventDetails.Details ?? "No details provided");
            htmlContent = htmlContent.Replace("{CreatedAt}", eventDetails.CreatedAt.ToString("dddd, dd MMMM yyyy"));

            var eventTypeDescription = EmailSubjectFactory.CreateSubject(
                (NotificationEventType)Enum.Parse(typeof(NotificationEventType), eventDetails.EventType), 
                eventDetails.Message);

            htmlContent = htmlContent.Replace("{EventType}", eventTypeDescription);

            string fullName = string.IsNullOrEmpty(student.FirstName) && string.IsNullOrEmpty(student.LastName) 
                ? "User" 
                : $"{student.FirstName} {student.LastName}".Trim();

            htmlContent = htmlContent.Replace("{UserName}", fullName);

            string userEmailConsentMessage = student.EmailNotifications 
                ? "You are receiving this message because you provided consent in MiniSpace Services user settings to receive notifications." 
                : "You are receiving this message due to a system requirement, as you have not provided consent in MiniSpace Services user settings.";

            htmlContent = htmlContent.Replace("{UserEmailConsent}", userEmailConsentMessage);


            return htmlContent;
        }

    }
}
