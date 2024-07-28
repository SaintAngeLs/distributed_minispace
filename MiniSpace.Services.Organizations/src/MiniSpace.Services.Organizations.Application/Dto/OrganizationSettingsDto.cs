using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class OrganizationSettingsDto
    {
        public bool IsVisible { get; set; }
        public bool IsPublic { get; set; }
        public bool IsPrivate { get; set; }
        public bool CanAddComments { get; set; }
        public bool CanAddReactions { get; set; }
        public bool CanPostPosts { get; set; }
        public bool CanPostEvents { get; set; }
        public bool CanMakeReposts { get; set; }
        public bool CanAddCommentsToPosts { get; set; }
        public bool CanAddReactionsToPosts { get; set; }
        public bool CanAddCommentsToEvents { get; set; }
        public bool CanAddReactionsToEvents { get; set; }
        public bool DisplayFeedInMainOrganization { get; set; }

        public OrganizationSettingsDto()
        {
            
        }

        public OrganizationSettingsDto(OrganizationSettings settings)
        {
            IsVisible = settings.IsVisible;
            IsPublic = settings.IsPublic;
            IsPrivate = settings.IsPrivate;
            CanAddComments = settings.CanAddComments;
            CanAddReactions = settings.CanAddReactions;
            CanPostPosts = settings.CanPostPosts;
            CanPostEvents = settings.CanPostEvents;
            CanMakeReposts = settings.CanMakeReposts;
            CanAddCommentsToPosts = settings.CanAddCommentsToPosts;
            CanAddReactionsToPosts = settings.CanAddReactionsToPosts;
            CanAddCommentsToEvents = settings.CanAddCommentsToEvents;
            CanAddReactionsToEvents = settings.CanAddReactionsToEvents;
            DisplayFeedInMainOrganization = settings.DisplayFeedInMainOrganization;
        }
    }
}
