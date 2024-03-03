using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Parcels.Core.Repositories;

namespace Pacco.Services.Parcels.Application.Events.External.Handlers
{
    public class ParcelAddedToOrderHandler : IEventHandler<ParcelAddedToOrder>
    {
        private readonly IParcelRepository _parcelRepository;

        public ParcelAddedToOrderHandler(IParcelRepository parcelRepository)
        {
            _parcelRepository = parcelRepository;
        }

        public async Task HandleAsync(ParcelAddedToOrder @event)
        {
            var parcel = await _parcelRepository.GetAsync(@event.ParcelId);
            if (parcel is null)
            {
                return;
            }

            parcel.AddToOrder(@event.OrderId);
            await _parcelRepository.UpdateAsync(parcel);
        }
    }
}