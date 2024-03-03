using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Parcels.Application.Events;
using Pacco.Services.Parcels.Application.Exceptions;
using Pacco.Services.Parcels.Application.Services;
using Pacco.Services.Parcels.Core.Exceptions;
using Pacco.Services.Parcels.Core.Repositories;

namespace Pacco.Services.Parcels.Application.Commands.Handlers
{
    public class DeleteParcelHandler : ICommandHandler<DeleteParcel>
    {
        private readonly IParcelRepository _parcelRepository;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;

        public DeleteParcelHandler(IParcelRepository parcelRepository, IAppContext appContext,
            IMessageBroker messageBroker)
        {
            _parcelRepository = parcelRepository;
            _appContext = appContext;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(DeleteParcel command)
        {
            var parcel = await _parcelRepository.GetAsync(command.ParcelId);
            if (parcel is null)
            {
                throw new ParcelNotFoundException(command.ParcelId);
            }

            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != parcel.CustomerId && !identity.IsAdmin)
            {
                throw new UnauthorizedParcelAccessException(parcel.Id, identity.Id);
            }

            if (parcel.AddedToOrder)
            {
                throw new CannotDeleteParcelException(command.ParcelId);
            }

            await _parcelRepository.DeleteAsync(command.ParcelId);
            await _messageBroker.PublishAsync(new ParcelDeleted(command.ParcelId));
        }
    }
}