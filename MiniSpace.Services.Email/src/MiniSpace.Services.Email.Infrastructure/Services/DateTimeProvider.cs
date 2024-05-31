using MiniSpace.Services.Email.Application.Services;

namespace MiniSpace.Services.Email.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
