using System;

namespace Pacco.Services.Customers.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}