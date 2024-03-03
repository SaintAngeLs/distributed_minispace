using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Pacco.Services.Vehicles.Application.DTO;
using Pacco.Services.Vehicles.Application.Queries;
using Pacco.Services.Vehicles.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Vehicles.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class SearchVehiclesHandler : IQueryHandler<SearchVehicles, PagedResult<VehicleDto>>
    {
        private readonly IMongoRepository<VehicleDocument, Guid> _repository;

        public SearchVehiclesHandler(IMongoRepository<VehicleDocument, Guid> repository)
            => _repository = repository;

        public async Task<PagedResult<VehicleDto>> HandleAsync(SearchVehicles query)
        {
            PagedResult<VehicleDocument> pagedResult;
            if (query.PayloadCapacity <= 0 && query.LoadingCapacity <= 0 && query.Variants <= 0)
            {
                pagedResult = await _repository.BrowseAsync(_ => true, query);
            }
            else
            {
                pagedResult = await _repository.BrowseAsync(v => v.PayloadCapacity >= query.PayloadCapacity
                                                                 && v.LoadingCapacity >= query.LoadingCapacity &&
                                                                 v.Variants == query.Variants, query);
            }


            return pagedResult?.Map(d => d.AsDto());
        }
    }
}