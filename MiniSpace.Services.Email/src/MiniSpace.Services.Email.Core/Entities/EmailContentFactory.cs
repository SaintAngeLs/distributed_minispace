using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Email.Core.Entities
{
    public class EmailContentFactory
    {
        private static readonly Dictionary<NotificationEventType, IEmailContentStrategy> Strategies =
            new Dictionary<NotificationEventType, IEmailContentStrategy>
            {
                { NotificationEventType.TwoFactorCodeGenerated, new TwoFactorCodeGeneratedContent() },
            };

        public static string CreateContent(NotificationEventType eventType, string details)
        {
            if (Strategies.TryGetValue(eventType, out var strategy))
            {
                return strategy.GenerateContent(details);
            }
            var defaultStrategy = new DefaultEmailContentStrategy();
            return defaultStrategy.GenerateContent(details);
        }
    }
}
