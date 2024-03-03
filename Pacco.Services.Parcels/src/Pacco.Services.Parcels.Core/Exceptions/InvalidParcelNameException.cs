namespace Pacco.Services.Parcels.Core.Exceptions
{
    public class InvalidParcelNameException : DomainException
    {
        public override string Code { get; } = "invalid_parcel_name";

        public InvalidParcelNameException(string name) : base($"Parcel name is invalid: {name}.")
        {
        }
    }
}