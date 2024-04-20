using MiniSpace.Services.Posts.Application.Services;

namespace MiniSpace.Services.Posts.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
