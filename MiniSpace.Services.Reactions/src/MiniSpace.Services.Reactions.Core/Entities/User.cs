namespace MiniSpace.Services.Reactions.Core.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public User(Guid id)
        {
            Id = id;
        }
    }
}
