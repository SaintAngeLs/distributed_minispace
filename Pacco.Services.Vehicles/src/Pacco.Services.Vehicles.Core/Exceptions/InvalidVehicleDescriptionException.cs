namespace Pacco.Services.Vehicles.Core.Exceptions
{
    public class InvalidVehicleDescriptionException : DomainException
    {
        public override string Code { get; } = "invalid_vehicle_description";

        public InvalidVehicleDescriptionException(string description)
            : base($"Vehicle description is invalid: {description}.")
        {
        }
    }
}