using MiniSpace.Services.Organizations.Application.Services;

namespace MiniSpace.Services.Organizations.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
