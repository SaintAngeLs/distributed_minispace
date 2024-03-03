using System;

namespace Pacco.Services.Orders.Application.Exceptions
{
    public class UnauthorizedOrderAccessException : AppException
    {
        public override string Code { get; } = "unauthorized_order_access";

        public UnauthorizedOrderAccessException(Guid id, Guid customerId) 
            : base($"Unauthorized access to order: '{id}' by customer: '{customerId}'")
        {
        }
    }
}