namespace MiniSpace.Services.Events.Core.Entities
{
    public class Address(
        string buildingName,
        string street,
        string buildingNumber,
        string apartmentNumber,
        string city,
        string zipCode,
        string country) 
    {
        public string BuildingName { get; set; } = buildingName;
        public string Street { get; set; } = street;
        public string BuildingNumber { get; set; } = buildingNumber;
        public string ApartmentNumber { get; set; } = apartmentNumber;
        public string City { get; set; } = city;
        public string ZipCode { get; set; } = zipCode;
        public string Country { get; set; } = country; 

        public Address Update(string buildingName, string street, string buildingNumber, string apartmentNumber,
            string city, string zipCode, string country) 
        {
            BuildingName = buildingName == string.Empty ? BuildingName : buildingName;
            Street = street == string.Empty ? Street : street;
            BuildingNumber = buildingNumber == string.Empty ? BuildingNumber : buildingNumber;
            ApartmentNumber = apartmentNumber == string.Empty ? ApartmentNumber : apartmentNumber;
            City = city == string.Empty ? City : city;
            ZipCode = zipCode == string.Empty ? ZipCode : zipCode;
            Country = country == string.Empty ? Country : country; // Update Country if provided
            return this;
        }
    }
}
