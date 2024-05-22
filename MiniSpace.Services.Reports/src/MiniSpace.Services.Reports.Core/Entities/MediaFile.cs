namespace MiniSpace.Services.Reports.Core.Entities
{
    public class MediaFile
    {
        public Guid Id { get; private set; }

        public MediaFile(Guid id)
        {
            Id = id;
        }
    }
}