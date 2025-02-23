using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Events.External;
using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Logging;

namespace MiniSpace.Services.Students.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(UpdateUser),  new HandlerLogTemplate
                    {
                        After = "Updated the student with id: {UserId}."
                    }
                },
                {
                    typeof(DeleteUser), new HandlerLogTemplate
                    {
                        After = "Deleted the student with id: {UserId}."
                    }
                },
                {
                    typeof(CompleteUserRegistration), new HandlerLogTemplate
                    {
                        After = "Completed a registration for the student with id: {UserId}."
                    }
                },
                {
                    typeof(ChangeUserState), new HandlerLogTemplate
                    {
                        After = "Changed a student with id: {UserId} state to: {State}."
                    }
                },
                {
                    typeof(SignedUp), new HandlerLogTemplate
                    {
                        After = "Created a new student with id: {UserId}."
                    }
                },
                {
                    typeof(UserShowedInterestInEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {UserId} has been interested in the event with id: {EventId}."
                    }
                },
                {
                    typeof(UserCancelledInterestInEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {UserId} has cancelled interest in the event with id: {EventId}."
                    }
                },
                {
                    typeof(UserSignedUpToEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {UserId} has signed up for the event with id: {EventId}."
                    }
                },
                {
                    typeof(UserCancelledSignUpToEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {UserId} has cancelled sign up for the event with id: {EventId}."
                    }
                },
                {
                    typeof(UserBanned), new HandlerLogTemplate
                    {
                        After = "A student with id: {UserId} has been banned."
                    }
                },
                {
                    typeof(UserUnbanned), new HandlerLogTemplate
                    {
                        After = "A student with id: {UserId} has been unbanned."
                    }
                }
            };
        
        public HandlerLogTemplate Map<TMessage>(TMessage message) where TMessage : class
        {
            var key = message.GetType();
            return MessageTemplates.TryGetValue(key, out var template) ? template : null;
        }
    }
}
