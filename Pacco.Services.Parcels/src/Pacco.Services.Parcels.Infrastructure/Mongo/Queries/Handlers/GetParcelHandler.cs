using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Pacco.Services.Parcels.Application.DTO;
using Pacco.Services.Parcels.Application.Queries;
using Pacco.Services.Parcels.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Parcels.Infrastructure.Mongo.Queries.Handlers
{
    public class GetParcelHandler : IQueryHandler<GetParcel, ParcelDto>
    {
        private readonly IMongoRepository<ParcelDocument, Guid> _parcelRepository;

        public GetParcelHandler(IMongoRepository<ParcelDocument, Guid> parcelRepository)
        {
            _parcelRepository = parcelRepository;
        }

        public async Task<ParcelDto> HandleAsync(GetParcel query)
        {
            var document = await _parcelRepository.GetAsync(p => p.Id == query.ParcelId);

            return document?.AsDto();
        }
    }
}