using System.Collections.Generic;

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
        
        public static string GetReactionIcon(this ReactionType? reactionType)
        {
            return reactionType switch
            {
                ReactionType.LoveIt => "heart",
                ReactionType.LikeIt => "thumb-up",
                ReactionType.Wow => "face-surprise",
                ReactionType.ItWasOkay => "fa-face-meh",
                ReactionType.HateIt => "face-tired",
                _ => "no-reactions"
            };
        }

        public static List<KeyValuePair<string, ReactionType?>> GenerateReactionPairs()
        => [
            new KeyValuePair<string, ReactionType?>("", null),
            new KeyValuePair<string, ReactionType?>(GetReactionText(ReactionType.LoveIt), ReactionType.LoveIt),
            new KeyValuePair<string, ReactionType?>(GetReactionText(ReactionType.LikeIt), ReactionType.LikeIt),
            new KeyValuePair<string, ReactionType?>(GetReactionText(ReactionType.Wow), ReactionType.Wow),
            new KeyValuePair<string, ReactionType?>(GetReactionText(ReactionType.ItWasOkay), ReactionType.ItWasOkay),
            new KeyValuePair<string, ReactionType?>(GetReactionText(ReactionType.HateIt), ReactionType.HateIt)
        ];
    }
}
