using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Parcels.Application.Commands
{
    [Contract]
    public class DeleteParcel : ICommand
    {
        public Guid ParcelId { get; }

        public DeleteParcel(Guid parcelId)
        {
            ParcelId = parcelId;
        }
    }
}