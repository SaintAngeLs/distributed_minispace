using System;

namespace Pacco.Services.Orders.Application.Events.Rejected
{
    [Contract]
    public class DeleteParcelFromOrderRejected
    {
        public Guid OrderId { get; }
        public Guid ParcelId { get; }
        public string Reason { get; }
        public string Code { get; }

        public DeleteParcelFromOrderRejected(Guid orderId, Guid parcelId, string reason, string code)
        {
            OrderId = orderId;
            ParcelId = parcelId;
            Reason = reason;
            Code = code;
        }
    }
}