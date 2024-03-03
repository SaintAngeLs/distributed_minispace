using System.Threading.Tasks;
using Chronicle;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Pacco.Services.OrderMaker.Commands;
using Pacco.Services.OrderMaker.Events.External;

namespace Pacco.Services.OrderMaker.Handlers
{
    public class AIOrderMakingHandler : 
        ICommandHandler<MakeOrder>, 
        IEventHandler<OrderApproved>, 
        IEventHandler<OrderCreated>, 
        IEventHandler<ParcelAddedToOrder>,
        IEventHandler<VehicleAssignedToOrder>,
        IEventHandler<ResourceReserved>
    {
        private readonly ISagaCoordinator _coordinator;

        public AIOrderMakingHandler(ISagaCoordinator coordinator)
        {
            _coordinator = coordinator;
        }
        
        public Task HandleAsync(MakeOrder command)
            => _coordinator.ProcessAsync(command, SagaContext.Empty);

        public Task HandleAsync(OrderApproved @event)
            => _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public Task HandleAsync(OrderCreated @event)
            => _coordinator.ProcessAsync(@event, SagaContext.Empty);
        
        public Task HandleAsync(ParcelAddedToOrder @event)
            => _coordinator.ProcessAsync(@event, SagaContext.Empty);
        
        public Task HandleAsync(VehicleAssignedToOrder @event)
            => _coordinator.ProcessAsync(@event, SagaContext.Empty);

        public Task HandleAsync(ResourceReserved @event)
            => _coordinator.ProcessAsync(@event, SagaContext.Empty);
    }
}