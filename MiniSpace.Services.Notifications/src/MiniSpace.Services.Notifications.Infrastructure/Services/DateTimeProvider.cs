using MiniSpace.Services.Notifications.Application.Services;

namespace MiniSpace.Services.Notifications.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
