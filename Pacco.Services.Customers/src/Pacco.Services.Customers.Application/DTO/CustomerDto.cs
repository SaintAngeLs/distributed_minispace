using System;

namespace Pacco.Services.Customers.Application.DTO
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}