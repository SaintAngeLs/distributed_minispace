using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Parcels.Core.Repositories;

namespace Pacco.Services.Parcels.Application.Events.External.Handlers
{
    public class OrderCanceledHandler : IEventHandler<OrderCanceled>
    {
        private readonly IParcelRepository _parcelRepository;

        public OrderCanceledHandler(IParcelRepository parcelRepository)
        {
            _parcelRepository = parcelRepository;
        }

        public async Task HandleAsync(OrderCanceled @event)
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