using Convey.Types;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class StudentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }    
}
