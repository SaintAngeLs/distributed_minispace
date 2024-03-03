using Pacco.Services.Customers.Application.DTO;
using Pacco.Services.Customers.Core.Entities;

namespace Pacco.Services.Customers.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static Customer AsEntity(this CustomerDocument document)
            => new Customer(document.Id, document.Email, document.CreatedAt, document.FullName, document.Address,
                document.IsVip, document.State, document.CompletedOrders);

        public static CustomerDocument AsDocument(this Customer entity)
            => new CustomerDocument
            {
                Id = entity.Id,
                Email = entity.Email,
                FullName = entity.FullName,
                Address = entity.Address,
                IsVip = entity.IsVip,
                State = entity.State,
                CreatedAt = entity.CreatedAt,
                CompletedOrders = entity.CompletedOrders
            };

        public static CustomerDto AsDto(this CustomerDocument document)
            => new CustomerDto
            {
                Id = document.Id,
                State = document.State.ToString().ToLowerInvariant(),
                CreatedAt = document.CreatedAt,
            };

        public static CustomerDetailsDto AsDetailsDto(this CustomerDocument document)
            => new CustomerDetailsDto
            {
                Id = document.Id,
                Email = document.Email,
                FullName = document.FullName,
                Address = document.Address,
                IsVip = document.IsVip,
                State = document.State.ToString().ToLowerInvariant(),
                CreatedAt = document.CreatedAt,
                CompletedOrders = document.CompletedOrders
            };
    }
}