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
                    typeof(EventInterestedIn), new HandlerLogTemplate
                    {
                        After = "A student with id: {StudentId} has been interested in the event with id: {EventId}."
                    }
                },
                {
                    typeof(EventSignedUp), new HandlerLogTemplate
                    {
                        After = "A student with id: {StudentId} has signed up for the event with id: {EventId}."
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
