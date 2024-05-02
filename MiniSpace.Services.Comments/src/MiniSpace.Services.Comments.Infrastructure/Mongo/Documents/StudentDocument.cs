using Convey.Types;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    public class StudentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
    }    
}
