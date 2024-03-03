using System.Threading.Tasks;
using Convey.CQRS.Events;
using Pacco.Services.Parcels.Core.Repositories;

namespace Pacco.Services.Parcels.Application.Events.External.Handlers
{
    public class ParcelDeletedFromOrderHandler : IEventHandler<ParcelDeletedFromOrder>
    {
        private readonly IParcelRepository _parcelRepository;

        public ParcelDeletedFromOrderHandler(IParcelRepository parcelRepository)
        {
            _parcelRepository = parcelRepository;
        }

        public async Task HandleAsync(ParcelDeletedFromOrder @event)
        {
            var parcel = await _parcelRepository.GetAsync(@event.ParcelId);
            if (parcel is null)
            {
                return;
            }

            parcel.DeleteFromOrder();
            await _parcelRepository.UpdateAsync(parcel);
        }
    }
}