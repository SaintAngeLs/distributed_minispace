using MiniSpace.Services.Reports.Application.Services;

namespace MiniSpace.Services.Reports.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
