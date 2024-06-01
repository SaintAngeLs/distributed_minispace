using System;
using System.Diagnostics.CodeAnalysis;
using Convey.MessageBrokers.RabbitMQ;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Events.Rejected;
using MiniSpace.Services.Events.Application.Exceptions;

namespace MiniSpace.Services.Events.Infrastructure.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                // TODO: Add more exceptions
                AuthorizedUserIsNotAnOrganizerException ex => new CreateEventRejected(ex.UserId, ex.Message, ex.Code),
                EventNotFoundException ex 
                    => message switch
                    {
                        AddEventParticipant m => new AddEventParticipantRejected(ex.EventId, m.StudentId, ex.Message, ex.Code),
                        CancelInterestInEvent m => new CancelInterestInEventRejected(ex.EventId, ex.Message, ex.Code),
                        CancelSignUpToEvent m => new CancelSignUpToEventRejected(ex.EventId, ex.Message, ex.Code),
                        DeleteEvent m => new DeleteEventRejected(ex.EventId, ex.Message, ex.Code),
                        RateEvent m => new RateEventRejected(ex.EventId, m.StudentId, ex.Message, ex.Code),
                        RemoveEventParticipant m => new RemoveEventParticipantRejected(ex.EventId, ex.Message, ex.Code),
                        ShowInterestInEvent m => new ShowInterestInEventRejected(ex.EventId, m.StudentId, ex.Message, ex.Code),
                        SignUpToEvent m => new SignUpToEventRejected(ex.EventId, m.StudentId, ex.Message, ex.Code),
                        UpdateEvent m => new UpdateEventRejected(ex.EventId, ex.Message, ex.Code),
                        _ => null
                    },    
                InvalidEventCategoryException ex 
                    => message switch
                    {
                        CreateEvent m => new CreateEventRejected(m.OrganizerId, ex.Message, ex.Code),
                        _ => null
                    },
                InvalidEventDateTimeException ex 
                    => message switch
                    {
                        CreateEvent m => new CreateEventRejected(m.OrganizerId, ex.Message, ex.Code),
                        _ => null
                    },
                InvalidEventDateTimeOrderException ex
                    => message switch
                    {
                        CreateEvent m => new CreateEventRejected(m.OrganizerId, ex.Message, ex.Code),
                        _ => null
                    },
                OrganizerCannotAddEventForAnotherOrganizerException ex
                    => message switch
                    {
                        CreateEvent m => new CreateEventRejected(m.OrganizerId, ex.Message, ex.Code),
                        _ => null
                    },
                StudentNotFoundException ex 
                    => message switch
                    {
                        AddEventParticipant m => new AddEventParticipantRejected(m.EventId, ex.StudentId, ex.Message, ex.Code),
                        ShowInterestInEvent m => new ShowInterestInEventRejected(m.EventId, ex.StudentId, ex.Message, ex.Code),
                        SignUpToEvent m => new SignUpToEventRejected(m.EventId, ex.StudentId, ex.Message, ex.Code),
                        RateEvent m => new RateEventRejected(m.EventId, ex.StudentId, ex.Message, ex.Code),
                        _ => null
                    },
                UnauthorizedEventAccessException ex 
                    => message switch
                    { 
                        AddEventParticipant m => new AddEventParticipantRejected(ex.EventId, m.StudentId, ex.Message, ex.Code),
                        CancelInterestInEvent m => new CancelInterestInEventRejected(ex.EventId, ex.Message, ex.Code),
                        CancelSignUpToEvent m => new CancelSignUpToEventRejected(ex.EventId, ex.Message, ex.Code),
                        DeleteEvent m => new DeleteEventRejected(ex.EventId, ex.Message, ex.Code),
                        RemoveEventParticipant m => new RemoveEventParticipantRejected(ex.EventId, ex.Message, ex.Code),
                        ShowInterestInEvent m => new ShowInterestInEventRejected(ex.EventId, m.StudentId, ex.Message, ex.Code),
                        SignUpToEvent m => new SignUpToEventRejected(ex.EventId, m.StudentId, ex.Message, ex.Code),
                        UpdateEvent m => new UpdateEventRejected(ex.EventId, ex.Message, ex.Code),
                        _ => null  
                    },
                _ => null,
            };
    }
}