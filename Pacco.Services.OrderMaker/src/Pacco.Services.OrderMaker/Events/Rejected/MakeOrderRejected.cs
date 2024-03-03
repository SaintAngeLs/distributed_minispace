using System;
using Convey.CQRS.Events;

namespace Pacco.Services.OrderMaker.Events.Rejected
{
    public class MakeOrderRejected : IRejectedEvent
    {
        public Guid OrderId { get; }
        public string Reason { get; }
        public string Code { get; }

        public MakeOrderRejected(Guid orderId, string reason, string code)
        {
            OrderId = orderId;
            Reason = reason;
            Code = code;
        }
    }
}