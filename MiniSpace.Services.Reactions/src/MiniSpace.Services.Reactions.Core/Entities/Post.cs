

namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class Post(Guid id)
    {
        public Guid Id { get; private set; } = id;
    }    
}
