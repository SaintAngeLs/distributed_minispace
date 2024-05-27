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
        private readonly IMessageBroker _messageBroker;

        public CreateEmailNotificationHandler(IStudentEmailsRepository studentEmailsRepository, IMessageBroker messageBroker)
        {
            _studentEmailsRepository = studentEmailsRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CreateEmailNotification command, CancellationToken cancellationToken = default)
        {
            var studentEmails = await _studentEmailsRepository.GetByStudentIdAsync(command.UserId);
            if (studentEmails == null)
            {
                // Assume you handle the case where the student doesn't exist yet.
                // Depending on the domain rules, you might want to create a new record.
                return;
            }

            var emailNotification = new EmailNotification(
                Guid.NewGuid(),
                command.UserId,
                command.EmailAddress,
                command.Subject,
                command.Body,
                EmailNotificationStatus.Pending
            );

            studentEmails.AddEmailNotification(emailNotification);
            await _studentEmailsRepository.UpdateAsync(studentEmails);

            // Optionally, notify other parts of the system an email has been created
            // This could be a separate event such as EmailNotificationCreated if needed.
        }
    }
}
