using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using Pacco.Services.Vehicles.Application.Commands;

namespace Pacco.Services.Vehicles.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddVehicle),     
                    new HandlerLogTemplate
                    {
                        After = "Added a vehicle with id: {VehicleId}."
                    }
                },
                {
                    typeof(DeleteVehicle),     
                    new HandlerLogTemplate
                    {
                        After = "Deleted a vehicle with id: {VehicleId}."
                    }
                },
                {
                    typeof(UpdateVehicle),     
                    new HandlerLogTemplate
                    {
                        After = "Updated a vehicle with id: {VehicleId}."
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