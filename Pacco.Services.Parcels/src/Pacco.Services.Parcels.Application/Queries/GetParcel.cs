using System;
using Convey.CQRS.Queries;
using Pacco.Services.Parcels.Application.DTO;

namespace Pacco.Services.Parcels.Application.Queries
{
    public class GetParcel : IQuery<ParcelDto>
    {
        public Guid ParcelId { get; set; }
    }
}