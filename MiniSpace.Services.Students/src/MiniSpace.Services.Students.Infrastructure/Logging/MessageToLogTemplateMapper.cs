using Convey.Logging.CQRS;
using MiniSpace.Services.Students.Application.Commands;
using MiniSpace.Services.Students.Application.Events.External;

namespace MiniSpace.Services.Students.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(UpdateStudent),  new HandlerLogTemplate
                    {
                        After = "Updated the student with id: {StudentId}."
                    }
                },
                {
                    typeof(DeleteStudent), new HandlerLogTemplate
                    {
                        After = "Deleted the student with id: {StudentId}."
                    }
                },
                {
                    typeof(CompleteStudentRegistration), new HandlerLogTemplate
                    {
                        After = "Completed a registration for the student with id: {StudentId}."
                    }
                },
                {
                    typeof(ChangeStudentState), new HandlerLogTemplate
                    {
                        After = "Changed a student with id: {StudentId} state to: {State}."
                    }
                },
                {
                    typeof(SignedUp), new HandlerLogTemplate
                    {
                        After = "Created a new student with id: {UserId}."
                    }
                },
                {
                    typeof(StudentShowedInterestInEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {StudentId} has been interested in the event with id: {EventId}."
                    }
                },
                {
                    typeof(StudentCancelledInterestInEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {StudentId} has cancelled interest in the event with id: {EventId}."
                    }
                },
                {
                    typeof(StudentSignedUpToEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {StudentId} has signed up for the event with id: {EventId}."
                    }
                },
                {
                    typeof(StudentCancelledSignUpToEvent), new HandlerLogTemplate
                    {
                        After = "A student with id: {StudentId} has cancelled sign up for the event with id: {EventId}."
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
                },
                {
                    typeof(OrganizerRightsGranted), new HandlerLogTemplate
                    {
                        After = "Organizer rights has been granted for student with id: {UserId}."
                    }
                },
                {
                    typeof(OrganizerRightsRevoked), new HandlerLogTemplate
                    {
                        After = "Organizer rights has been revoked for student with id: {UserId}."
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
