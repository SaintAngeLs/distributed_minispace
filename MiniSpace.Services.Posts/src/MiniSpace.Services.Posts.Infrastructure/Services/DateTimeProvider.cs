using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Posts.Application.Services;

namespace MiniSpace.Services.Posts.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
