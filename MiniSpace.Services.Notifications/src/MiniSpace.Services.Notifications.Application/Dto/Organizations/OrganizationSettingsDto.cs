
namespace MiniSpace.Services.Notifications.Application.Dto.Organizations
{
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
    }
}
