using Convey.Types;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public class MediaFileDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}