using System;

namespace Pacco.Services.Orders.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}