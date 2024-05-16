using Convey.Logging.CQRS;
using MiniSpace.Services.Organizations.Application.Commands;
using MiniSpace.Services.Organizations.Application.Events.External;

namespace MiniSpace.Services.Organizations.Infrastructure.Logging
{
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateOrganization),  new HandlerLogTemplate
                    {
                        After = "Added a new organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(AddOrganizerToOrganization), new HandlerLogTemplate
                    {
                        After = "Added an organizer with id: {OrganizerId} to the organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(RemoveOrganizerFromOrganization), new HandlerLogTemplate
                    {
                        After = "Removed an organizer with id: {OrganizerId} from the organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(OrganizerRightsGranted), new HandlerLogTemplate
                    {
                        After = "Created an organizer with id: {UserId}."
                    }
                },
                {
                    typeof(OrganizerRightsRevoked), new HandlerLogTemplate
                    {
                        After = "Deleted an organizer with id: {UserId}."
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
