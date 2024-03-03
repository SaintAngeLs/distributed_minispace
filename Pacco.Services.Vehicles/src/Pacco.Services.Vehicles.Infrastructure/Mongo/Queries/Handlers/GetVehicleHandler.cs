using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using Pacco.Services.Vehicles.Application.DTO;
using Pacco.Services.Vehicles.Application.Queries;
using Pacco.Services.Vehicles.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Vehicles.Infrastructure.Mongo.Queries.Handlers
{
    internal sealed class GetVehicleHandler : IQueryHandler<GetVehicle, VehicleDto>
    {
        private readonly IMongoRepository<VehicleDocument, Guid> _repository;

        public GetVehicleHandler(IMongoRepository<VehicleDocument, Guid> repository)
            => _repository = repository;
        
        public async Task<VehicleDto> HandleAsync(GetVehicle query)
        {
            var document = await _repository.GetAsync(v => v.Id == query.VehicleId);
            return document?.AsDto();
        }
    }
}