namespace MiniSpace.Services.Events.Core.Entities
{
    public class Reaction
    {
        public string StudentId { get; set; }
        public ReactionType Type { get; set; }
    }
    
    public enum ReactionType
    {
        Like,
        Okay,
        Hate,
        Wow,
        Love
    }
}