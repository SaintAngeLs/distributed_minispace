using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Orders.Core.Events;
using Pacco.Services.Orders.Core.Exceptions;

namespace Pacco.Services.Orders.Core.Entities
{
    public class Order : AggregateRoot
    {
        private ISet<Parcel> _parcels = new HashSet<Parcel>();
        public Guid CustomerId { get; private set; }
        public Guid? VehicleId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeliveryDate { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string CancellationReason { get; private set; }
        public bool CanBeDeleted => Status == OrderStatus.New;
        public bool CanAssignVehicle => Status == OrderStatus.New || Status == OrderStatus.Canceled;
        public bool HasParcels => Parcels.Any();

        public IEnumerable<Parcel> Parcels
        {
            get => _parcels;
            private set => _parcels = new HashSet<Parcel>(value);
        }

        public Order(AggregateId id, Guid customerId, OrderStatus status, DateTime createdAt,
            IEnumerable<Parcel> parcels = null, Guid? vehicleId = null, DateTime? deliveryDate = null,
            decimal totalPrice = 0)
        {
            Id = id;
            CustomerId = customerId;
            Status = status;
            CreatedAt = createdAt;
            Parcels = parcels ?? Enumerable.Empty<Parcel>();
            if (vehicleId.HasValue)
            {
                SetVehicle(vehicleId.Value);
            }

            if (deliveryDate.HasValue)
            {
                SetDeliveryDate(deliveryDate.Value);
            }

            TotalPrice = totalPrice;
            CancellationReason = string.Empty;
        }

        public static Order Create(AggregateId id, Guid customerId, OrderStatus status, DateTime createdAt)
        {
            var order = new Order(id, customerId, status, createdAt);
            order.AddEvent(new OrderStateChanged(order));

            return order;
        }

        public void SetTotalPrice(decimal totalPrice)
        {
            if (Status != OrderStatus.New)
            {
                throw new CannotChangeOrderPriceException(Id);
            }

            if (totalPrice < 0)
            {
                throw new InvalidOrderPriceException(Id, totalPrice);
            }

            TotalPrice = totalPrice;
        }

        public void SetVehicle(Guid vehicleId)
        {
            VehicleId = vehicleId;
        }

        public void SetDeliveryDate(DateTime deliveryDate)
        {
            DeliveryDate = deliveryDate.Date;
        }

        public void AddParcel(Parcel parcel)
        {
            if (!_parcels.Add(parcel))
            {
                throw new ParcelAlreadyAddedToOrderException(Id, parcel.Id);
            }

            AddEvent(new ParcelAdded(this, parcel));
        }

        public void DeleteParcel(Guid parcelId)
        {
            var parcel = _parcels.SingleOrDefault(p => p.Id == parcelId);
            if (parcel is null)
            {
                throw new OrderParcelNotFoundException(parcelId, Id);
            }

            AddEvent(new ParcelDeleted(this, parcel));
        }

        public void Approve()
        {
            if (Status != OrderStatus.New && Status != OrderStatus.Canceled)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Approved);
            }

            Status = OrderStatus.Approved;
            CancellationReason = string.Empty;
            AddEvent(new OrderStateChanged(this));
        }

        public void Cancel(string reason)
        {
            if (Status == OrderStatus.Completed || Status == OrderStatus.Canceled)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Canceled);
            }

            Status = OrderStatus.Canceled;
            CancellationReason = reason ?? string.Empty;
            AddEvent(new OrderStateChanged(this));
        }

        public void Complete()
        {
            if (Status != OrderStatus.Delivering)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Completed);
            }

            Status = OrderStatus.Completed;
            AddEvent(new OrderStateChanged(this));
        }

        public void SetDelivering()
        {
            if (Status != OrderStatus.Approved)
            {
                throw new CannotChangeOrderStateException(Id, Status, OrderStatus.Delivering);
            }

            Status = OrderStatus.Delivering;
            AddEvent(new OrderStateChanged(this));
        }
    }
}