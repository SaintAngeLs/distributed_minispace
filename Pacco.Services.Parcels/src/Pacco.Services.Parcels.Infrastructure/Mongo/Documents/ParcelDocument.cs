using System;
using Convey.Types;
using Pacco.Services.Parcels.Core.Entities;

namespace Pacco.Services.Parcels.Infrastructure.Mongo.Documents
{
    public class ParcelDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Variant Variant { get; set; }
        public Size Size { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? OrderId { get; set; }
        public bool AddedToOrder { get; set; }
    }
}