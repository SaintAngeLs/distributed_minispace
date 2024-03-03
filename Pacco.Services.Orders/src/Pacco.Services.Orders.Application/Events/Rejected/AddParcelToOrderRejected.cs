using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Orders.Application.Events.Rejected
{
    [Contract]
    public class AddParcelToOrderRejected: IRejectedEvent
    {
        public Guid OrderId { get; }
        public Guid ParcelId { get; }
        public string Reason { get; }
        public string Code { get; }

        public AddParcelToOrderRejected(Guid orderId, Guid parcelId, string reason, string code)
        {
            OrderId = orderId;
            ParcelId = parcelId;
            Reason = reason;
            Code = code;
        }
    }
}