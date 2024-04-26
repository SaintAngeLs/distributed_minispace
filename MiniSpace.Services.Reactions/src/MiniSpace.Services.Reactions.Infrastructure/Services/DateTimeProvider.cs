using MiniSpace.Services.Reactions.Application.Services;

namespace MiniSpace.Services.Reactions.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
