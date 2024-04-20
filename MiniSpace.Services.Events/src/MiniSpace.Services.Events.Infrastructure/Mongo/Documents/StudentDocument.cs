using System;
using Convey.Types;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    public class StudentDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}