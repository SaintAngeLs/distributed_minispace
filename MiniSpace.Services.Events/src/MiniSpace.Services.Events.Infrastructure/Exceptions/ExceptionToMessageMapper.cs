using System;
using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Events.Application.Events.Rejected;
using MiniSpace.Services.Events.Application.Exceptions;

namespace MiniSpace.Services.Events.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                // TODO: Add more exceptions
                AuthorizedUserIsNotAnOrganizerException ex => new AddEventRejected(ex.UserId, ex.Message, ex.Code),
                OrganizerCannotAddEventForAnotherOrganizerException ex => new AddEventRejected(ex.OrganizerId, ex.Message, ex.Code),
                InvalidEventCategoryException ex => new AddEventRejected(Guid.Empty, ex.Message, ex.Code),
                InvalidEventDateTimeException ex => new AddEventRejected(Guid.Empty, ex.Message, ex.Code),
                InvalidEventDateTimeOrderException ex => new AddEventRejected(Guid.Empty, ex.Message, ex.Code),
                _ => null,
            };
    }
}