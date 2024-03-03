using System;

namespace Pacco.Services.Parcels.Core.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }

        public Customer(Guid id)
        {
            Id = id;
        }
    }
}