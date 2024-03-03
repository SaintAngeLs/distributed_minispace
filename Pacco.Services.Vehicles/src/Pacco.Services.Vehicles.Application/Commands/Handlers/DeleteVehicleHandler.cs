using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Vehicles.Application.Events;
using Pacco.Services.Vehicles.Application.Exceptions;
using Pacco.Services.Vehicles.Application.Services;
using Pacco.Services.Vehicles.Core.Repositories;

namespace Pacco.Services.Vehicles.Application.Commands.Handlers
{
    internal class DeleteVehicleHandler : ICommandHandler<DeleteVehicle>
    {
        private readonly IVehiclesRepository _repository;
        private readonly IMessageBroker _broker;

        public DeleteVehicleHandler(IVehiclesRepository repository, IMessageBroker broker)
        {
            _repository = repository;
            _broker = broker;
        }
        
        public async Task HandleAsync(DeleteVehicle command)
        {
            var vehicle = await _repository.GetAsync(command.VehicleId);

            if (vehicle is null)
            {
                throw new VehicleNotFoundException(command.VehicleId);
            }
            
            await _repository.DeleteAsync(vehicle);
            await _broker.PublishAsync(new VehicleDeleted(command.VehicleId));
        }
    }
}