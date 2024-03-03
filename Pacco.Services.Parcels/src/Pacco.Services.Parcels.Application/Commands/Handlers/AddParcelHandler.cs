using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Pacco.Services.Parcels.Application.Events;
using Pacco.Services.Parcels.Application.Exceptions;
using Pacco.Services.Parcels.Application.Services;
using Pacco.Services.Parcels.Core.Entities;
using Pacco.Services.Parcels.Core.Exceptions;
using Pacco.Services.Parcels.Core.Repositories;

namespace Pacco.Services.Parcels.Application.Commands.Handlers
{
    public class AddParcelHandler : ICommandHandler<AddParcel>
    {
        private readonly IParcelRepository _parcelRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AddParcelHandler(IParcelRepository parcelRepository, ICustomerRepository customerRepository,
            IMessageBroker messageBroker, IDateTimeProvider dateTimeProvider)
        {
            _parcelRepository = parcelRepository;
            _customerRepository = customerRepository;
            _messageBroker = messageBroker;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task HandleAsync(AddParcel command)
        {
            if (!Enum.TryParse<Variant>(command.Variant, true, out var variant))
            {
                throw new InvalidParcelVariantException(command.Variant);
            }

            if (!Enum.TryParse<Size>(command.Size, true, out var size))
            {
                throw new InvalidParcelSizeException(command.Size);
            }

            if (!(await _customerRepository.ExistsAsync(command.CustomerId)))
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }

            var parcel = new Parcel(command.ParcelId, command.CustomerId, variant, size, command.Name,
                command.Description, _dateTimeProvider.Now);
            await _parcelRepository.AddAsync(parcel);
            await _messageBroker.PublishAsync(new ParcelAdded(command.ParcelId));
        }
    }
}

