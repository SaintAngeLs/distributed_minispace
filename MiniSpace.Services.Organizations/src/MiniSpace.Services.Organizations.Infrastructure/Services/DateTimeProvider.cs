using MiniSpace.Services.Organizations.Application.Services;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    internal sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }    
}
