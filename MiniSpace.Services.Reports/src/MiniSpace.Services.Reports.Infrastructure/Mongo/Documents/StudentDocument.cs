using System;
using Convey.Types;

namespace MiniSpace.Services.Reports.Infrastructure.Mongo.Documents
{
    public class StudentDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}