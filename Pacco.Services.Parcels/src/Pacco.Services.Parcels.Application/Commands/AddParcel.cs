using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Parcels.Application.Commands
{
    [Contract]
    public class AddParcel : ICommand
    {
        public Guid ParcelId { get; }
        public Guid CustomerId { get; }
        public string Variant { get; }
        public string Size { get; }
        public string Name { get; }
        public string Description { get; }

        public AddParcel(Guid parcelId, Guid customerId, string variant, string size, string name, string description)
        {
            ParcelId = parcelId == Guid.Empty ? Guid.NewGuid() : parcelId;
            CustomerId = customerId;
            Variant = variant;
            Size = size;
            Name = name;
            Description = description;
        }
    }
}