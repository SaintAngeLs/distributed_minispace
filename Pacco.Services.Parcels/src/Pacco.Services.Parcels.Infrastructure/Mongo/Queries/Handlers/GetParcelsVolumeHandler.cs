using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Pacco.Services.Parcels.Application.DTO;
using Pacco.Services.Parcels.Application.Queries;
using Pacco.Services.Parcels.Core.Services;
using Pacco.Services.Parcels.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Parcels.Infrastructure.Mongo.Queries.Handlers
{
    public class GetParcelsVolumeHandler : IQueryHandler<GetParcelsVolume, ParcelsVolumeDto>
    {
        private readonly IMongoRepository<ParcelDocument, Guid> _parcelRepository;
        private readonly IParcelsService _parcelsService;

        public GetParcelsVolumeHandler(IMongoRepository<ParcelDocument, Guid> parcelRepository,
            IParcelsService parcelsService)
        {
            _parcelRepository = parcelRepository;
            _parcelsService = parcelsService;
        }

        public async Task<ParcelsVolumeDto> HandleAsync(GetParcelsVolume query)
        {
            if (query.ParcelIds is null || !query.ParcelIds.Any())
            {
                return new ParcelsVolumeDto
                {
                    Volume = 0
                };
            }

            var documents = await _parcelRepository.FindAsync(p => query.ParcelIds.Contains(p.Id));
            var parcels = documents.Select(d => d.AsEntity());
            var volume = _parcelsService.CalculateVolume(parcels);

            return new ParcelsVolumeDto
            {
                Volume = volume
            };
        }
    }
}