using MiniSpace.Services.Students.Application.Services;

namespace MiniSpace.Services.Students.Infrastructure.Services
{
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
