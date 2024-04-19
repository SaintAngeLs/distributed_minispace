using System;
using System.Collections.Generic;
using Convey.Logging.CQRS;
using MiniSpace.Services.Events.Application.Commands;
using MiniSpace.Services.Events.Application.Events;
using MiniSpace.Services.Events.Application.Events.External;
using MiniSpace.Services.Events.Application.Exceptions;

namespace MiniSpace.Services.Events.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(AddEvent),     
                    new HandlerLogTemplate
                    {
                        After = "Added an event with id: {EventId}."
                    }
                },
                {
                    typeof(DeleteEvent),     
                    new HandlerLogTemplate
                    {
                        After = "Event with id: {EventId} has been deleted."
                    }
                },
                {
                    typeof(RateEvent),     
                    new HandlerLogTemplate
                    {
                        After = "Rated an event with id: {EventId} with rating: {Rating}."
                    }
                },
                {
                    typeof(SearchEvents),     
                    new HandlerLogTemplate
                    {
                        After = "Searching events with name: {Name}, organizer: {Organizer}."
                    }
                },
                {
                    typeof(ShowInterestInEvent),     
                    new HandlerLogTemplate
                    {
                        After = "Student with id: {StudentId} showed interest in event with id: {EventId}."
                    }
                },
                {
                    typeof(SignUpToEvent),     
                    new HandlerLogTemplate
                    {
                        After = "Student with id: {StudentId} signed up to event with id: {EventId}."
                    }
                },
                {
                    typeof(EventBackgroundWorkerStarted),     
                    new HandlerLogTemplate
                    {
                        After = "Background worker with name: {Name} has started."
                    }
                },
                {
                    typeof(EventBackgroundWorkerStopped),     
                    new HandlerLogTemplate
                    {
                        After = "Background worker with name: {Name} has stopped."
                    }
                },
                {
                    typeof(StudentCreated),     
                    new HandlerLogTemplate
                    {
                        After = "Added a student with id: {StudentId}",
                        OnError = new Dictionary<Type, string>
                        {
                            {
                                typeof(StudentAlreadyAddedException), 
                                "Student with id: {StudentId} was already added."
                                
                            }
                        }
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