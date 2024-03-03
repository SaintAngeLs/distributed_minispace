using System;
using Convey.CQRS.Queries;
using Pacco.Services.Deliveries.Application.DTO;

namespace Pacco.Services.Deliveries.Application.Queries
{
    public class GetDelivery : IQuery<DeliveryDto>
    { 
        public Guid DeliveryId { get; set; }
    }
}