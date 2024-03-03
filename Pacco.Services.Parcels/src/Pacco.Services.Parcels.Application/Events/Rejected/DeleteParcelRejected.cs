using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Parcels.Application.Events.Rejected
{
    [Contract]
    public class DeleteParcelRejected : IRejectedEvent
    {
        public Guid ParcelId { get; }
        public string Reason { get; }
        public string Code { get; }

        public DeleteParcelRejected(Guid parcelId, string reason, string code)
        {
            ParcelId = parcelId;
            Reason = reason;
            Code = code;
        }
    }
}