using Convey.Types;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public class CommentDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}