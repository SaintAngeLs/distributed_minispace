using System;
using Pacco.Services.Deliveries.Application.Services;

namespace Pacco.Services.Deliveries.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}