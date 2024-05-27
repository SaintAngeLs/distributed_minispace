using Convey.Types;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public class PostDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}