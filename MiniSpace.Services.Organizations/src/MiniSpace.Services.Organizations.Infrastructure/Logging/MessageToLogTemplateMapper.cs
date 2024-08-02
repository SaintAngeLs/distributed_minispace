using Convey.Logging.CQRS;
using MiniSpace.Services.Organizations.Application.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Logging
{
    [ExcludeFromCodeCoverage]
    internal sealed class MessageToLogTemplateMapper : IMessageToLogTemplateMapper
    {
        private static IReadOnlyDictionary<Type, HandlerLogTemplate> MessageTemplates 
            => new Dictionary<Type, HandlerLogTemplate>
            {
                {
                    typeof(CreateOrganization), new HandlerLogTemplate
                    {
                        After = "Added a new organization with id: {OrganizationId}, name: {Name}."
                    }
                },
                {
                    typeof(DeleteOrganization), new HandlerLogTemplate
                    {
                        After = "Deleted an organization with id: {OrganizationId} and its children."
                    }
                },
                {
                    typeof(AssignRoleToMember), new HandlerLogTemplate
                    {
                        After = "Assigned role '{Role}' to member with id: {MemberId} in organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(CreateOrganizationRole), new HandlerLogTemplate
                    {
                        After = "Created a new role '{RoleName}' in organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(CreateSubOrganization), new HandlerLogTemplate
                    {
                        After = "Created a new sub-organization with id: {SubOrganizationId} under parent organization with id: {ParentId}."
                    }
                },
                {
                    typeof(InviteUserToOrganization), new HandlerLogTemplate
                    {
                        After = "Invited user with id: {UserId} to organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(ManageFeed), new HandlerLogTemplate
                    {
                        After = "Performed '{Action}' action on the feed content in organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(SetOrganizationPrivacy), new HandlerLogTemplate
                    {
                        After = "Set privacy of organization with id: {OrganizationId} to '{IsPrivate}'."
                    }
                },
                {
                    typeof(SetOrganizationVisibility), new HandlerLogTemplate
                    {
                        After = "Set visibility of organization with id: {OrganizationId} to '{IsVisible}'."
                    }
                },
                {
                    typeof(UpdateOrganizationSettings), new HandlerLogTemplate
                    {
                        After = "Updated settings of organization with id: {OrganizationId}."
                    }
                },
                {
                    typeof(UpdateRolePermissions), new HandlerLogTemplate
                    {
                        After = "Updated permissions for role with id: {RoleId} in organization with id: {OrganizationId}."
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
