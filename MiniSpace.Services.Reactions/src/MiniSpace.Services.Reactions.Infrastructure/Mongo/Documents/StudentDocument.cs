using Convey.Types;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    public class StudentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }    
}
