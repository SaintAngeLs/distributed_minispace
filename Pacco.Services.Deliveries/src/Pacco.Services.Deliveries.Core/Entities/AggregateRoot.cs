using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Deliveries.Core.Events;

namespace Pacco.Services.Deliveries.Core.Entities
{
    public interface IAggregateRoot
    {
        IEnumerable<IDomainEvent> Events { get; }
        AggregateId Id { get;  }
        int Version { get; }
        void IncrementVersion();
    }
    
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        public IEnumerable<IDomainEvent> Events => _events;
        public AggregateId Id { get; protected set; }
        public int Version { get; protected set; }
        public bool HasChanged => _events.Any();

        protected void AddEvent(IDomainEvent @event)
        {
            _events.Add(@event);
        }

        public void ClearEvents() => _events.Clear();

        void IAggregateRoot.IncrementVersion()
            => Version++;
    }
}

