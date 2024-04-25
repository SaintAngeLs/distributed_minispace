using MiniSpace.Services.Friends.Application.Services;

namespace MiniSpace.Services.Friends.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
