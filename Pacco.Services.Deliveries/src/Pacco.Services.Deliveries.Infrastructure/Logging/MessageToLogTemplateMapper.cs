using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using Pacco.Services.Deliveries.Application.Commands;

namespace Pacco.Services.Deliveries.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddDeliveryRegistration), new HandlerLogTemplate
                    {
                        After = "Added a registration for the delivery with id: {DeliveryId}."
                    }
                },
                {
                    typeof(CompleteDelivery),  new HandlerLogTemplate
                    {
                        After = "Completed the delivery with id: {DeliveryId}."
                    }
                },
                {
                    typeof(FailDelivery), new HandlerLogTemplate
                    {
                        After = "Failed the delivery with id: {DeliveryId}, reason: {Reason}"
                    }
                },
                {
                    typeof(StartDelivery), new HandlerLogTemplate
                    {
                        After = "Started the delivery with id: {DeliveryId}."
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