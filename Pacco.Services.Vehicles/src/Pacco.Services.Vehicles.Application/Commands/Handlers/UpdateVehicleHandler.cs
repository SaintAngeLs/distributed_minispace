using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Vehicles.Application.Events;
using Pacco.Services.Vehicles.Application.Exceptions;
using Pacco.Services.Vehicles.Application.Services;
using Pacco.Services.Vehicles.Core.Repositories;

namespace Pacco.Services.Vehicles.Application.Commands.Handlers
{
    internal class UpdateVehicleHandler : ICommandHandler<UpdateVehicle>
    {
        private readonly IVehiclesRepository _repository;
        private readonly IMessageBroker _broker;

        public UpdateVehicleHandler(IVehiclesRepository repository, IMessageBroker broker)
        {
            _repository = repository;
            _broker = broker;
        }

        public async Task HandleAsync(UpdateVehicle command)
        {
            var vehicle = await _repository.GetAsync(command.VehicleId);

            if (vehicle is null)
            {
                throw new VehicleNotFoundException(command.VehicleId);
            }

            vehicle.ChangeDescription(command.Description);
            vehicle.ChangePricePerService(command.PricePerService);
            vehicle.ChangeVariants(command.Variants);
            await _repository.UpdateAsync(vehicle);
            await _broker.PublishAsync(new VehicleUpdated(command.VehicleId));
        }
    }
}