using Convey.CQRS.Commands;
using MiniSpace.Services.Email.Core.Repositories;
using MiniSpace.Services.Email.Core.Entities;
using System;
using System.Threading.Tasks;
using MiniSpace.Services.Email.Application.Services;

namespace MiniSpace.Services.Email.Application.Commands.Handlers
{
    public class CreateEmailNotificationHandler : ICommandHandler<CreateEmailNotification>
    {
        private readonly IStudentEmailsRepository _studentEmailsRepository;
        private readonly IEmailService _emailService;
        private readonly IMessageBroker _messageBroker;

        public CreateEmailNotificationHandler(IStudentEmailsRepository studentEmailsRepository, IMessageBroker messageBroker, IEmailService emailService) 
        {
            _studentEmailsRepository = studentEmailsRepository;
            _messageBroker = messageBroker;
            _emailService = emailService; 
        }


        public async Task HandleAsync(CreateEmailNotification command, CancellationToken cancellationToken = default)
        {
            var studentEmails = await _studentEmailsRepository.GetByStudentIdAsync(command.UserId);
            if (studentEmails == null)
            {
                // Handle the case where the student doesn't exist yet.
                // return;
            }

            var emailNotification = new EmailNotification(
                Guid.NewGuid(),
                command.UserId,
                command.EmailAddress,
                command.Subject,
                command.Body,
                EmailNotificationStatus.Pending
            );

            // studentEmails.AddEmailNotification(emailNotification);
            // await _studentEmailsRepository.UpdateAsync(studentEmails);

            // Send the email using the email service
            await _emailService.SendEmailAsync(command.EmailAddress, command.Subject, command.Body);
        }
    }
}
