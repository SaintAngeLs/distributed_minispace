using System;
using System.Collections.Generic;
using MiniSpace.Services.Email.Core.Entities;

namespace MiniSpace.Services.Email.Application.Dto
{
    public class StudentEmailsDto
    {
        private List<EmailNotification> notifications = new List<EmailNotification>();
        public Guid StudentId { get; private set; }

        public StudentEmailsDto(Guid studentId)
        {
            StudentId = studentId;
        }

        public void AddNotification(EmailNotification notification)
        {
            notifications.Add(notification);
        }

        public IEnumerable<EmailNotification> Notifications => notifications.AsReadOnly();
    }
}
