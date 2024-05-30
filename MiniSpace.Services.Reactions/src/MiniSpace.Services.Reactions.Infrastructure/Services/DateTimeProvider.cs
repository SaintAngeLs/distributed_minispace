using System.Diagnostics.CodeAnalysis;
using MiniSpace.Services.Reactions.Application.Services;

namespace MiniSpace.Services.Reactions.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
