using System;

namespace Pacco.Services.Parcels.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}