using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Vehicles.Application.Events
{
    [Contract]
    public class VehicleUpdated : IEvent
    {
        public Guid VehicleId { get; }

        public VehicleUpdated(Guid id)
            => VehicleId = id;
    }
}