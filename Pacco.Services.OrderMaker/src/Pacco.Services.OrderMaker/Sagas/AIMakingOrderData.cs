using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacco.Services.OrderMaker.Sagas
{
    public class AIMakingOrderData
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ReservationPriority { get; set; }
        public List<Guid> ParcelIds { get; set; } = new List<Guid>();
        public List<Guid> AddedParcelIds { get; set; } = new List<Guid>();
        public bool AllPackagesAddedToOrder => AddedParcelIds.Any() && AddedParcelIds.All(ParcelIds.Contains);
    }
}