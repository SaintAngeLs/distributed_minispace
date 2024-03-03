using System;

namespace Pacco.Services.Pricing.Api.Core.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public bool IsVip { get; private set; }
        public int CompletedOrdersNumber { get; private set; }
        
        public Customer(Guid id, bool isVip, int completedOrdersNumber)
        {
            IsVip = isVip;
            CompletedOrdersNumber = completedOrdersNumber;
        }
    }
}