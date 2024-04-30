namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class Organizer
    {
        public Guid Id { get; private set; }
        
        public Organizer(Guid id)
        {
            Id = id;
        }
    }
}