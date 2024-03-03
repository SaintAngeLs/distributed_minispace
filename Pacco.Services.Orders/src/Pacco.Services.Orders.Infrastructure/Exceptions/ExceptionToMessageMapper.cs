using System;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Orders.Application.Commands;
using Pacco.Services.Orders.Application.Events.External;
using Pacco.Services.Orders.Application.Events.Rejected;
using Pacco.Services.Orders.Application.Exceptions;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                CannotDeleteOrderException ex => (object) new DeleteOrderRejected(ex.OrderId, ex.Message, ex.Code),
                CustomerNotFoundException ex => new CreateOrderRejected(ex.CustomerId, ex.Message, ex.Code),
                OrderForReservedVehicleNotFoundException ex => new OrderForReservedVehicleNotFound(ex.VehicleId,
                    ex.Date, ex.Message, ex.Code),

                OrderNotFoundException ex
                => message switch
                {
                    AddParcelToOrder m => (object) new AddParcelToOrderRejected(m.OrderId, m.ParcelId, ex.Message,
                        ex.Code),
                    ApproveOrder m => new ApproveOrderRejected(m.OrderId, ex.Message, ex.Code),
                    AssignVehicleToOrder m => new AssignVehicleToOrderRejected(m.OrderId, m.VehicleId, ex.Message,
                        ex.Code),
                    CancelOrder m => new CancelOrderRejected(m.OrderId, ex.Message, ex.Code),
                    DeleteOrder m => new DeleteOrderRejected(m.OrderId, ex.Message, ex.Code),
                    DeleteParcelFromOrder m => new DeleteParcelFromOrderRejected(m.OrderId, m.ParcelId, ex.Message,
                        ex.Code),
                    DeliveryCompleted _ => new OrderForDeliveryNotFound(ex.Id, ex.Message, ex.Code),
                    DeliveryFailed _ => new OrderForDeliveryNotFound(ex.Id, ex.Message, ex.Code),
                    DeliveryStarted _ => new OrderForDeliveryNotFound(ex.Id, ex.Message, ex.Code),
                    _ => null
                },

                OrderHasNoParcelsException ex
                => message switch
                {
                    AddParcelToOrder m => new AssignVehicleToOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code),
                    _ => null
                },

                OrderParcelNotFoundException ex
                => message switch
                {
                    AddParcelToOrder m => new AddParcelToOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code),
                    _ => null
                },

                ParcelNotFoundException ex
                => message switch
                {
                    AddParcelToOrder m => new AddParcelToOrderRejected(m.OrderId, m.ParcelId, ex.Message, ex.Code),
                    _ => null
                },

                ParcelAlreadyAddedToOrderException ex => new AddParcelToOrderRejected(ex.OrderId, ex.ParcelId,
                    ex.Message, ex.Code),

                UnauthorizedOrderAccessException ex
                => message switch
                {
                    AddParcelToOrder m => (object) new AddParcelToOrderRejected(m.OrderId, m.ParcelId, ex.Message,
                        ex.Code),
                    AssignVehicleToOrder m => new AssignVehicleToOrderRejected(m.OrderId, m.VehicleId, ex.Message,
                        ex.Code),
                    DeleteOrder m => new DeleteOrderRejected(m.OrderId, ex.Message, ex.Code),
                    DeleteParcelFromOrder m => new DeleteParcelFromOrderRejected(m.OrderId, m.ParcelId, ex.Message,
                        ex.Code),
                    _ => null
                },
                VehicleNotFoundException ex
                => message switch
                {
                    AssignVehicleToOrder m => new AssignVehicleToOrderRejected(m.OrderId, m.VehicleId, ex.Message,
                        ex.Code),
                    _ => null,
                },
                _ => null,
            };
    }
}