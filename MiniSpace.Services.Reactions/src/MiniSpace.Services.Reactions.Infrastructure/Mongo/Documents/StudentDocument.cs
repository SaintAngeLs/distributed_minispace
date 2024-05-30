using System.Diagnostics.CodeAnalysis;
using Convey.Types;

namespace MiniSpace.Services.Reactions.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class StudentDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }    
}
