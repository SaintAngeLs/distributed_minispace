using System;
using Pacco.Services.Orders.Application.Services;

namespace Pacco.Services.Orders.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}