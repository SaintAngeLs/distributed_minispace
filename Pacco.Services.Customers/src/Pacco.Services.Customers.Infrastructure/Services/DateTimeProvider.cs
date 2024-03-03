using System;
using Pacco.Services.Customers.Application.Services;

namespace Pacco.Services.Customers.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}