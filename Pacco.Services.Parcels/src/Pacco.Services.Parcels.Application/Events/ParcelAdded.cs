using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Parcels.Application.Events
{
    [Contract]
    public class ParcelAdded : IEvent
    {
        public Guid ParcelId { get; }

        public ParcelAdded(Guid parcelId)
        {
            ParcelId = parcelId;
        }
    }
}