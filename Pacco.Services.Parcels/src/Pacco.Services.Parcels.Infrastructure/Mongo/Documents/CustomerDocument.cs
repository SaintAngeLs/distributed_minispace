using System;
using Convey.Types;

namespace Pacco.Services.Parcels.Infrastructure.Mongo.Documents
{
    public class CustomerDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
    }
}