using System;

namespace MiniSpace.Services.Comments.Application.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }    
}
