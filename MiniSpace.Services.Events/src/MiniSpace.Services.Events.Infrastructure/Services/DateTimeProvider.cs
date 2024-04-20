using System;
using MiniSpace.Services.Events.Application.Services;

namespace MiniSpace.Services.Events.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}