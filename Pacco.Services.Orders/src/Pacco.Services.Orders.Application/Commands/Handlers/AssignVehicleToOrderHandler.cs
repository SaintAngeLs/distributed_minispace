using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Orders.Application.Events;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Application.Services;
using Pacco.Services.Orders.Application.Services.Clients;
using Pacco.Services.Orders.Core.Repositories;

namespace Pacco.Services.Orders.Application.Commands.Handlers
{
    public class AssignVehicleToOrderHandler : ICommandHandler<AssignVehicleToOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPricingServiceClient _pricingServiceClient;
        private readonly IVehiclesServiceClient _vehiclesServiceClient;
        private readonly IMessageBroker _messageBroker;
        private readonly IAppContext _appContext;

        public AssignVehicleToOrderHandler(IOrderRepository orderRepository,
            IPricingServiceClient pricingServiceClient, IVehiclesServiceClient vehiclesServiceClient,
            IMessageBroker messageBroker, IAppContext appContext)
        {
            _orderRepository = orderRepository;
            _pricingServiceClient = pricingServiceClient;
            _vehiclesServiceClient = vehiclesServiceClient;
            _messageBroker = messageBroker;
            _appContext = appContext;
        }

        public async Task HandleAsync(AssignVehicleToOrder command)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != order.CustomerId && !identity.IsAdmin)
            {
                throw new UnauthorizedOrderAccessException(command.OrderId, identity.Id);
            }

            if (!order.HasParcels)
            {
                throw new OrderHasNoParcelsException(command.OrderId);
            }

            if (!order.CanAssignVehicle)
            {
                return;
            }

            var vehicle = await _vehiclesServiceClient.GetAsync(command.VehicleId);
            if (vehicle is null)
            {
                throw new VehicleNotFoundException(command.VehicleId);
            }
            
            var pricing = await _pricingServiceClient.GetOrderPriceAsync(order.CustomerId, vehicle.PricePerService);
            order.SetVehicle(command.VehicleId);
            order.SetTotalPrice(pricing.OrderDiscountPrice);
            order.SetDeliveryDate(command.DeliveryDate);
            await _orderRepository.UpdateAsync(order);
            await _messageBroker.PublishAsync(new VehicleAssignedToOrder(command.OrderId, command.VehicleId));
        }
    }
}