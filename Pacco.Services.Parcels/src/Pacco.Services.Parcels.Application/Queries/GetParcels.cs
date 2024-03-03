using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Pacco.Services.Parcels.Application.DTO;

namespace Pacco.Services.Parcels.Application.Queries
{
    public class GetParcels : IQuery<IEnumerable<ParcelDto>>
    {
        public Guid? CustomerId { get; set; }
        public bool IncludeAddedToOrders { get; set; }
    }
}