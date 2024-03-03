using System;

namespace Pacco.Services.Deliveries.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}