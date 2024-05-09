using MiniSpace.Services.Comments.Application.Services;

namespace MiniSpace.Services.Comments.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
