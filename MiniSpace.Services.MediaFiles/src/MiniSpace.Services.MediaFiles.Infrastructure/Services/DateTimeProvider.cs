using MiniSpace.Services.MediaFiles.Application.Services;

namespace MiniSpace.Services.MediaFiles.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
