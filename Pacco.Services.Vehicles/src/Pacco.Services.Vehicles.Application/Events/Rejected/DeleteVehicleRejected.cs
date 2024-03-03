using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Vehicles.Application.Events.Rejected
{
    [Contract]
    public class DeleteVehicleRejected : IRejectedEvent
    {
        public Guid VehicleId { get; }
        public string Reason { get; }
        public string Code { get; }

        public DeleteVehicleRejected(Guid vehicleId, string reason, string code)
        {
            VehicleId = vehicleId;
            Reason = reason;
            Code = code;
        }
    }
}