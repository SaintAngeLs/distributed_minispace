using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class PublishEvents : ICommand
    {
        public DateTime Now { get; }
        
        public PublishEvents(DateTime now)
        {
            Now = now;
        }
    }
}