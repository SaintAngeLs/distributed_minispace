using System.Collections.Generic;
using Pacco.Services.Parcels.Core.Entities;

namespace Pacco.Services.Parcels.Core.Services
{
    public interface IParcelsService
    {
        double CalculateVolume(IEnumerable<Parcel> parcels);
    }
}