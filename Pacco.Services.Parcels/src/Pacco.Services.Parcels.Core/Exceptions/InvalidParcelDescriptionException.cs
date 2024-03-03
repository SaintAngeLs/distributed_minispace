namespace Pacco.Services.Parcels.Core.Exceptions
{
    public class InvalidParcelDescriptionException : DomainException
    {
        public override string Code { get; } = "invalid_parcel_description";

        public InvalidParcelDescriptionException(string description)
            : base($"Parcel description is invalid: {description}.")
        {
        }
    }
}