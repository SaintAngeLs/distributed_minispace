using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSpace.Services.Emails.Core.Entities
{
    public class StudentEmails
    {
        public Guid StudentId { get; private set; }
        private List<EmailNotification> _emailNotifications;

        public IEnumerable<EmailNotification> EmailNotifications => _emailNotifications.AsReadOnly();

        public StudentEmails(Guid studentId)
        {
            StudentId = studentId;
            _emailNotifications = new List<EmailNotification>();
        }

        public void AddEmailNotification(EmailNotification emailNotification)
        {
            if (emailNotification != null)
            {
                _emailNotifications.Add(emailNotification);
            }
        }

        public void RemoveEmailNotification(Guid emailNotificationId)
        {
            _emailNotifications.RemoveAll(e => e.EmailNotificationId == emailNotificationId);
        }

        public void MarkEmailAsSent(Guid emailNotificationId)
        {
            var emailNotification = _emailNotifications.FirstOrDefault(e => e.EmailNotificationId == emailNotificationId);
            if (emailNotification != null)
            {
                emailNotification.MarkAsSent();
            }
        }
    }
}
