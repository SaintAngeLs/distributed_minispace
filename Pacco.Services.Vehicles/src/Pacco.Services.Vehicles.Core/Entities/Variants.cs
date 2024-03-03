using System;

namespace Pacco.Services.Vehicles.Core.Entities
{
    [Flags]
    public enum Variants
    {
        Standard =  1 << 0,
        Chemistry = 1 << 1,
        Weapon =    1 << 2,
        Animal =    1 << 3,
        Organ =     1 << 4
    }
}