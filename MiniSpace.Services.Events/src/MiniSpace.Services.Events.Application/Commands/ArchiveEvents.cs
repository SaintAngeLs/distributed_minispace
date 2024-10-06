using System;
using Paralax.CQRS.Commands;

namespace MiniSpace.Services.Events.Application.Commands
{
    public class ArchiveEvents : ICommand
    {
        public DateTime Now { get; }
        
        public ArchiveEvents(DateTime now)
        {
            Now = now;
        }
    }
}