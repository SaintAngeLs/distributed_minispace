using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Parcels.Core.Repositories;

namespace Pacco.Services.Parcels.Application.Events.External.Handlers
{
    public class OrderDeletedHandler : IEventHandler<OrderDeleted>
    {
        private readonly IParcelRepository _parcelRepository;

        public OrderDeletedHandler(IParcelRepository parcelRepository)
        {
            _parcelRepository = parcelRepository;
        }

        public async Task HandleAsync(OrderDeleted @event)
        {
            var parcel = await _parcelRepository.GetByOrderAsync(@event.OrderId);
            if (parcel is null)
            {
                return;
            }

            parcel.DeleteFromOrder();
            await _parcelRepository.UpdateAsync(parcel);
        }
    }
}