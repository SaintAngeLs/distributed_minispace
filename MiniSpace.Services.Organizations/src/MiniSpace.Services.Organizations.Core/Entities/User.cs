namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class User
    {
        public Guid Id { get; }

        public User(Guid id)
        {
            Id = id;
        }
    }

}