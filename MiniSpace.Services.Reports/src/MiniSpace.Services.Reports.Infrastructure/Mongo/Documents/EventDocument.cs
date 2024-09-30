using Paralax.Types;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public class EventDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}