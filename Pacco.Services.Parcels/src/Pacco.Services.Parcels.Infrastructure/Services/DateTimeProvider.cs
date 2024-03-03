using System;
using Pacco.Services.Parcels.Application.Services;

namespace Pacco.Services.Parcels.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}