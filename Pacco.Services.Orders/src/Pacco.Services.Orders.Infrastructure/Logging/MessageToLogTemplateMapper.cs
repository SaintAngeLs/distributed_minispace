using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using Pacco.Services.Orders.Application.Commands;
using Pacco.Services.Orders.Application.Events.External;
using Pacco.Services.Orders.Application.Exceptions;

namespace Pacco.Services.Orders.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddParcelToOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Added a parcel with id: {ParcelId} to the order with id: {OrderId}."
                    }
                },
                {
                    typeof(ApproveOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been approved."
                    }
                },
                {
                    typeof(AssignVehicleToOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Assigned a vehicle with id: {VehicleId} to the order with id: {OrderId}."
                    }
                },
                {
                    typeof(CancelOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been canceled."
                    }
                },
                {
                    typeof(CreateOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been created."
                    }
                },
                {
                    typeof(DeleteOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been deleted."
                    }
                },
                {
                    typeof(DeleteParcelFromOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Deleted a parcel with id: {ParcelId} from the order with id: {OrderId}."
                    }
                },
                {
                    typeof(CustomerCreated),     
                    new HandlerLogTemplate
                    {
                        After = "Added a customer with id: {CustomerId}",
                        OnError = new Dictionary<Type, string>
                        {
                            {
                                typeof(CustomerAlreadyAddedException), 
                                "Customer with id: {CustomerId} was already added."
                                
                            }
                        }
                    }
                },
                {
                    typeof(DeliveryCompleted),     
                    new HandlerLogTemplate
                    {
                        After = "Delivery with id: {DeliveryId} for the order with id: {OrderId} has been completed."
                    }
                },
                {
                    typeof(DeliveryFailed),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} has been canceled due to the failed delivery, reason: {Reason}"
                    }
                },
                {
                    typeof(DeliveryStarted),     
                    new HandlerLogTemplate
                    {
                        After = "Delivery for the order with id: {OrderId} has started"
                    }
                },
                {
                    typeof(ParcelDeleted),     
                    new HandlerLogTemplate
                    {
                        After = "Parcel with id: {ParcelId} has been deleted from the order"
                    }
                },
                {
                    typeof(ResourceReservationCanceled),     
                    new HandlerLogTemplate
                    {
                        After = "Reservation for the resource with id: {ResourceId}, date:  {DateTime} has been canceled."
                    }
                },
                {
                    typeof(ResourceReserved),     
                    new HandlerLogTemplate
                    {
                        After = "Reservation for the resource with id: {ResourceId}, date: {DateTime} has been made."
                    }
                },
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}