using System.Collections.Generic;

namespace Astravent.Web.Wasm.DTO.Enums
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
        private const string Heart = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24""><title>heart</title><path d=""M12,21.35L10.55,20.03C5.4,15.36 2,12.27 2,8.5C2,5.41 4.42,3 7.5,3C9.24,3 10.91,3.81 12,5.08C13.09,3.81 14.76,3 16.5,3C19.58,3 22,5.41 22,8.5C22,12.27 18.6,15.36 13.45,20.03L12,21.35Z"" /></svg>";
        private const string Like = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24""><title>thumb-up</title><path d=""M23,10C23,8.89 22.1,8 21,8H14.68L15.64,3.43C15.66,3.33 15.67,3.22 15.67,3.11C15.67,2.7 15.5,2.32 15.23,2.05L14.17,1L7.59,7.58C7.22,7.95 7,8.45 7,9V19A2,2 0 0,0 9,21H18C18.83,21 19.54,20.5 19.84,19.78L22.86,12.73C22.95,12.5 23,12.26 23,12V10M1,21H5V9H1V21Z"" /></svg>";
        private const string Okay = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24""><title>emoticon-happy</title><path d=""M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M7,9.5C7,8.7 7.7,8 8.5,8C9.3,8 10,8.7 10,9.5C10,10.3 9.3,11 8.5,11C7.7,11 7,10.3 7,9.5M12,17.23C10.25,17.23 8.71,16.5 7.81,15.42L9.23,14C9.68,14.72 10.75,15.23 12,15.23C13.25,15.23 14.32,14.72 14.77,14L16.19,15.42C15.29,16.5 13.75,17.23 12,17.23M15.5,11C14.7,11 14,10.3 14,9.5C14,8.7 14.7,8 15.5,8C16.3,8 17,8.7 17,9.5C17,10.3 16.3,11 15.5,11Z"" /></svg>";
        private const string Wow = @"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24""><title>emoticon-lol</title><path d=""M15.07 8.93V4.93A4.06 4.06 0 0 1 15.73 2.72A10 10 0 0 0 2.73 15.72A4.06 4.06 0 0 1 4.94 15.06H8.94V19.06A4.06 4.06 0 0 1 8.28 21.27A10 10 0 0 0 21.28 8.27A4.06 4.06 0 0 1 19.07 8.93M11 6H12.5V7.5H14V9H11M7.5 14V12.5H6V11H9V14M15.89 15.9A5.5 5.5 0 0 1 9.8 17L17 9.8A5.5 5.5 0 0 1 15.89 15.89M4.89 21.08A2 2 0 0 1 2.89 19.08A2 2 0 0 1 4.89 17.08H6.89V19.08A2 2 0 0 1 4.93 21.07M19.07 2.93A2 2 0 0 1 21.07 4.93A2 2 0 0 1 19.07 6.93H17.07V4.93A2 2 0 0 1 19.07 2.93Z"" /></svg>";
        private const string Hate =@"<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 24 24""><title>emoticon-confused</title><path d=""M12 2A10 10 0 1 0 22 12A10 10 0 0 0 12 2M8.5 8A1.5 1.5 0 1 1 7 9.5A1.54 1.54 0 0 1 8.5 8M17 16H13A4 4 0 0 0 9.53 18L7.8 17A6 6 0 0 1 13 14H17M15.5 11A1.5 1.5 0 1 1 17 9.5A1.54 1.54 0 0 1 15.5 11Z"" /></svg>";
        
        
        public static string GetReactionText(this ReactionType reactionType)
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
                ReactionType.LoveIt => Heart,
                ReactionType.LikeIt => Like,
                ReactionType.Wow => Wow,
                ReactionType.ItWasOkay => Okay,
                ReactionType.HateIt => Hate,
                _ => "no-reactions"
            };
        }
        
        public static string GetReactionIcon(this ReactionType reactionType)
        {
            return reactionType switch
            {
                ReactionType.LoveIt => Heart,
                ReactionType.LikeIt => Like,
                ReactionType.Wow => Wow,
                ReactionType.ItWasOkay => Okay,
                ReactionType.HateIt => Hate,
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
