using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Convey.Types;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class OrganizationRequestsDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public IEnumerable<RequestDocument> Requests { get; set; }
    }

    public class RequestDocument
    {
        public Guid RequestId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string State { get; set; }
        public string Reason { get; set; }
    }
}
