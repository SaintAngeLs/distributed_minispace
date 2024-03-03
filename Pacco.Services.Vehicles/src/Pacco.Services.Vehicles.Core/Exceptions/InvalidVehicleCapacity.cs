namespace Pacco.Services.Vehicles.Core.Exceptions
{
    public class InvalidVehicleCapacity : DomainException
    {
        public override string Code { get; } = "invalid_vehicle_capacity";

        public InvalidVehicleCapacity(double capacity)
            : base($"Vehicle capacity is invalid: {capacity}.")
        {
        }
    }
}