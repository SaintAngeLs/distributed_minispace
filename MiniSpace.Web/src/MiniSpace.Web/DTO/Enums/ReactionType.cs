namespace MiniSpace.Web.DTO.Enums
{
    public enum ReactionType
    {
        LoveIt,
        LikeIt,
        Wow,
        ItWasOkay,
        HateIt
    }

    public static class ReactionTypeExtensions
    {
        public static string GetReactionText(ReactionType? reactionType)
        {
            return reactionType switch
            {
                ReactionType.LoveIt => "Love it!",
                ReactionType.LikeIt => "Like it.",
                ReactionType.Wow => "Wow!",
                ReactionType.ItWasOkay => "It was okay.",
                ReactionType.HateIt => "Hate it!",
                _ => "No reactions!"
            };
        }   
    }
}
