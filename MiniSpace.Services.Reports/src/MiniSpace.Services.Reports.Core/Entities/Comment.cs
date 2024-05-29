namespace MiniSpace.Services.Reports.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }

        public Comment(Guid id)
        {
            Id = id;
        }
    }
}