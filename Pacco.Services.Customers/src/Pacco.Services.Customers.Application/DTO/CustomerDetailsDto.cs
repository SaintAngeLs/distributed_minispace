using System;
using System.Collections.Generic;

namespace Pacco.Services.Customers.Application.DTO
{
    public class CustomerDetailsDto : CustomerDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public bool IsVip { get; set; }
        public IEnumerable<Guid> CompletedOrders { get; set; }
    }
}