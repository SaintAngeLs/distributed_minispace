using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Parcels.Core.Entities;

namespace Pacco.Services.Parcels.Core.Services
{
    public class ParcelsService : IParcelsService
    {
        private readonly IReadOnlyDictionary<Size, double> _parcelSideLengths = new Dictionary<Size, double>
        {
            { Size.Tiny, 10},
            { Size.Small, 30},
            { Size.Normal, 50},
            { Size.Large, 75},
            { Size.Huge, 100},
            { Size.Exclusive, 200},
        };

        public double CalculateVolume(IEnumerable<Parcel> parcels)
            => parcels
                .Select(parcel => _parcelSideLengths[parcel.Size])
                .Select(CalculateParcelVolume)
                .Sum();
        
        private static double CalculateParcelVolume(double sideLength)
            => Math.Pow(sideLength, 3) / 1_000_000;
    }
}