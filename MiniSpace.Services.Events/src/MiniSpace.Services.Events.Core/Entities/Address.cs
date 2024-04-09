namespace MiniSpace.Services.Events.Core.Entities
{
    public class Address(
        string buildingName,
        string street,
        string buildingNumber,
        string apartmentNumber,
        string city,
        string zipCode)
    {
        public string BuildingName { get; set; } = buildingName;
        public string Street { get; set; } = street;
        public string BuildingNumber { get; set; } = buildingNumber;
        public string ApartmentNumber { get; set; } = apartmentNumber;
        public string City { get; set; } = city;
        public string ZipCode { get; set; } = zipCode;
    }
}