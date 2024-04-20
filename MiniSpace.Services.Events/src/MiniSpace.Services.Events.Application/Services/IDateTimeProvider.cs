using System;

namespace MiniSpace.Services.Events.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}