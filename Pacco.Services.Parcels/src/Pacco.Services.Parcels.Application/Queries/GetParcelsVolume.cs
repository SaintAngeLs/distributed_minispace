using System;
using System.Collections.Generic;
using Convey.CQRS.Queries;
using Pacco.Services.Parcels.Application.DTO;

namespace Pacco.Services.Parcels.Application.Queries
{
    public class GetParcelsVolume : IQuery<ParcelsVolumeDto>
    {
        public IEnumerable<Guid> ParcelIds { get; set; }
    }
}