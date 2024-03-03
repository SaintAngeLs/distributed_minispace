using System;
using System.Collections.Generic;
using Convey.Types;
using Pacco.Services.Customers.Core.Entities;

namespace Pacco.Services.Customers.Infrastructure.Mongo.Documents
{
    public class CustomerDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsVip { get; set; }
        public State State { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Guid> CompletedOrders { get; set; }
    }
}