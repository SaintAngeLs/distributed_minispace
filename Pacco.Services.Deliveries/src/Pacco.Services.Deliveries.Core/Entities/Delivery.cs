using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Deliveries.Core.Events;
using Pacco.Services.Deliveries.Core.Exceptions;
using Pacco.Services.Deliveries.Core.ValueObjects;

namespace Pacco.Services.Deliveries.Core.Entities
{
    public class Delivery : AggregateRoot
    {
        public Guid OrderId { get; protected set; }
        public DeliveryStatus Status { get; protected set; }
        public string Notes { get; protected set; }

        public IEnumerable<DeliveryRegistration> Registrations
        {
            get => _registrations;
            private set => _registrations = new HashSet<DeliveryRegistration>(value);
        }

        public DateTime? LastUpdate => Registrations
            .OrderByDescending(r => r.DateTime)
            .Select(r => r.DateTime)
            .FirstOrDefault();

        private ISet<DeliveryRegistration> _registrations = new HashSet<DeliveryRegistration>();

        public Delivery(AggregateId id, Guid orderId, DeliveryStatus status,
            IEnumerable<DeliveryRegistration> registrations = null)
        {
            Id = id;
            OrderId = orderId;
            Status = status;
            Registrations = registrations ?? Enumerable.Empty<DeliveryRegistration>();
        }

        public static Delivery Create(AggregateId id, Guid orderId, DeliveryStatus status)
        {
            var delivery = new Delivery(id, orderId, status);
            delivery.AddEvent(new DeliveryStateChanged(delivery));

            return delivery;
        }

        public void AddRegistration(DeliveryRegistration registration)
        {
            if (Status != DeliveryStatus.InProgress)
            {
                throw new CannotAddDeliveryRegistrationException(Id, Status);
            }

            if (!_registrations.Add(registration))
            {
                return;
            }

            AddEvent(new DeliveryRegistrationAdded(this, registration));
        }

        public void Complete()
        {
            if (Status is DeliveryStatus.Failed)
            {
                throw new CannotChangeDeliveryStateException(Id, Status, DeliveryStatus.Completed);
            }

            if (Status is DeliveryStatus.Completed)
            {
                throw new CannotChangeDeliveryStateException(Id, Status, DeliveryStatus.Completed);
            }

            Status = DeliveryStatus.Completed;
            AddEvent(new DeliveryStateChanged(this));
        }

        public void Fail(string reason)
        {
            if (Status is DeliveryStatus.Failed)
            {
                throw new CannotChangeDeliveryStateException(Id, Status, DeliveryStatus.Failed);
            }

            if (Status is DeliveryStatus.Completed)
            {
                throw new CannotChangeDeliveryStateException(Id, Status, DeliveryStatus.Failed);
            }

            Status = DeliveryStatus.Failed;
            Notes = reason;
            AddEvent(new DeliveryStateChanged(this));
        }
    }
}