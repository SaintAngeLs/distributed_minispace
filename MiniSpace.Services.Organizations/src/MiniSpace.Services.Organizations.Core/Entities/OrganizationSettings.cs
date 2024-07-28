namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class OrganizationSettings
    {
        public bool IsVisible { get; private set; }
        public bool IsPublic { get; private set; }
        public bool CanAddComments { get; private set; }
        public bool CanAddReactions { get; private set; }
        public bool CanPostPosts { get; private set; }
        public bool CanPostEvents { get; private set; }
        public bool CanMakeReposts { get; private set; }
        public bool CanAddCommentsToPosts { get; private set; }
        public bool CanAddReactionsToPosts { get; private set; }
        public bool CanAddCommentsToEvents { get; private set; }
        public bool CanAddReactionsToEvents { get; private set; }

        public OrganizationSettings(
            bool isVisible = true, 
            bool isPublic = true, 
            bool canAddComments = true, 
            bool canAddReactions = true, 
            bool canPostPosts = true, 
            bool canPostEvents = true, 
            bool canMakeReposts = true,
            bool canAddCommentsToPosts = true,
            bool canAddReactionsToPosts = true,
            bool canAddCommentsToEvents = true,
            bool canAddReactionsToEvents = true)
        {
            IsVisible = isVisible;
            IsPublic = isPublic;
            CanAddComments = canAddComments;
            CanAddReactions = canAddReactions;
            CanPostPosts = canPostPosts;
            CanPostEvents = canPostEvents;
            CanMakeReposts = canMakeReposts;
            CanAddCommentsToPosts = canAddCommentsToPosts;
            CanAddReactionsToPosts = canAddReactionsToPosts;
            CanAddCommentsToEvents = canAddCommentsToEvents;
            CanAddReactionsToEvents = canAddReactionsToEvents;
        }
 
        public void SetVisibility(bool isVisible)
        {
            IsVisible = isVisible;
        }

        public void SetPrivacy(bool isPublic)
        {
            IsPublic = isPublic;
        }

        public void SetCanAddComments(bool canAddComments)
        {
            CanAddComments = canAddComments;
        }

        public void SetCanAddReactions(bool canAddReactions)
        {
            CanAddReactions = canAddReactions;
        }

        public void SetCanPostPosts(bool canPostPosts)
        {
            CanPostPosts = canPostPosts;
        }

        public void SetCanPostEvents(bool canPostEvents)
        {
            CanPostEvents = canPostEvents;
        }

        public void SetCanMakeReposts(bool canMakeReposts)
        {
            CanMakeReposts = canMakeReposts;
        }

        public void SetCanAddCommentsToPosts(bool canAddCommentsToPosts)
        {
            CanAddCommentsToPosts = canAddCommentsToPosts;
        }

        public void SetCanAddReactionsToPosts(bool canAddReactionsToPosts)
        {
            CanAddReactionsToPosts = canAddReactionsToPosts;
        }

        public void SetCanAddCommentsToEvents(bool canAddCommentsToEvents)
        {
            CanAddCommentsToEvents = canAddCommentsToEvents;
        }

        public void SetCanAddReactionsToEvents(bool canAddReactionsToEvents)
        {
            CanAddReactionsToEvents = canAddReactionsToEvents;
        }
    }
}
