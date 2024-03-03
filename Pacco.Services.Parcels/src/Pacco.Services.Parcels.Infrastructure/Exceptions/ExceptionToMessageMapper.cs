using System;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Parcels.Application.Commands;
using Pacco.Services.Parcels.Application.Events.Rejected;
using Pacco.Services.Parcels.Application.Exceptions;
using Pacco.Services.Parcels.Core.Exceptions;

namespace Pacco.Services.Parcels.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                CannotDeleteParcelException ex => new DeleteParcelRejected(ex.Id, ex.Message, ex.Code),
                CustomerNotFoundException ex => new AddParcelRejected(ex.Message, ex.Code),
                InvalidParcelVariantException ex => new AddParcelRejected(ex.Message, ex.Code),
                InvalidParcelSizeException ex => new AddParcelRejected(ex.Message, ex.Code),
                InvalidParcelNameException ex => new AddParcelRejected(ex.Message, ex.Code),
                InvalidParcelDescriptionException ex => new AddParcelRejected(ex.Message, ex.Code),
                ParcelNotFoundException ex => (message is DeleteParcel
                    ? new DeleteParcelRejected(Guid.Empty, ex.Message, ex.Code)
                    : null),
                UnauthorizedParcelAccessException ex => new AddParcelRejected(ex.Message, ex.Code),
                _ => null
            };
    }
}