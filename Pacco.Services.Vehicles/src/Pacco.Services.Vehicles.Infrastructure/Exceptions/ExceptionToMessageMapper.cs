using System;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Vehicles.Application.Commands;
using Pacco.Services.Vehicles.Application.Events.Rejected;
using Pacco.Services.Vehicles.Application.Exceptions;
using Pacco.Services.Vehicles.Core.Exceptions;

namespace Pacco.Services.Vehicles.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                InvalidVehicleCapacity ex => message switch
                {
                    AddVehicle command => new AddVehicleRejected(command.VehicleId, ex.Message,ex.Code),
                    UpdateVehicle command => new UpdateVehicleRejected(command.VehicleId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidVehicleDescriptionException ex => message switch
                {
                    AddVehicle command => new AddVehicleRejected(command.VehicleId, ex.Message,ex.Code),
                    UpdateVehicle command => new UpdateVehicleRejected(command.VehicleId, ex.Message, ex.Code),
                    _ => null,
                },
                InvalidVehiclePricePerServiceException ex => message switch
                {
                    AddVehicle command => new AddVehicleRejected(command.VehicleId, ex.Message,ex.Code),
                    UpdateVehicle command => new UpdateVehicleRejected(command.VehicleId, ex.Message, ex.Code),
                    _ => null,
                },
                VehicleNotFoundException ex => message switch
                {
                    UpdateVehicle command => new UpdateVehicleRejected(command.VehicleId, ex.Message,ex.Code),
                    DeleteVehicle command => new DeleteVehicleRejected(command.VehicleId, ex.Message, ex.Code),
                    _ => null,
                },
                _ => null,
            };
    }
}