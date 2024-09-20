using MiniSpace.Services.Communication.Application.Services;

namespace MiniSpace.Services.Communication.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
