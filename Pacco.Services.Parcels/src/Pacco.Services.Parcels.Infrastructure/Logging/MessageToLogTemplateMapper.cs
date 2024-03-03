using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using Pacco.Services.Parcels.Application.Commands;
using Pacco.Services.Parcels.Application.Events.External;
using Pacco.Services.Parcels.Application.Exceptions;

namespace Pacco.Services.Parcels.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddParcel),     
                    new HandlerLogTemplate
                    {
                        After = "Added a parcel with id: {ParcelId} the customer: {CustomerId}"
                    }
                },
                {
                    typeof(DeleteParcel),     
                    new HandlerLogTemplate
                    {
                        After = "Deleted a parcel with id: {ParcelId}"
                    }
                }, 
                {
                    typeof(CustomerCreated),     
                    new HandlerLogTemplate
                    {
                        After = "Added a customer with id: {CustomerId}.",
                        OnError = new Dictionary<Type, string>
                        {
                            {
                                typeof(CustomerAlreadyExistsException), "Customer with id: {CustomerId} already exists."
                            }
                        }
                    }
                }, 
                {
                    typeof(OrderCanceled),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} was canceled. Parcels can be added to the new order again."
                    }
                }, 
                {
                    typeof(OrderDeleted),     
                    new HandlerLogTemplate
                    {
                        After = "Order with id: {OrderId} was deleted. Parcels can be added to the new order again."
                    }
                }, 
                {
                    typeof(ParcelAddedToOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Parcel with id: {ParcelId} was added to the order with id: {OrderId}"
                    }
                }, 
                {
                    typeof(ParcelDeletedFromOrder),     
                    new HandlerLogTemplate
                    {
                        After = "Parcel with id: {ParcelId} was deleted from the order with id: {OrderId}"
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