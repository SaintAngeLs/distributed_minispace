namespace MiniSpace.Services.Events.Core.Entities
{
    public class Address
    {
        public string BuildingName { get; private set; }
        public string Street { get; private set; }
        public string BuildingNumber { get; private set; }
        public string ApartmentNumber { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string Country { get; private set; }
        public double Latitude { get; private set; }  
        public double Longitude { get; private set; } 

        public Address(string buildingName, string street, string buildingNumber, string apartmentNumber, 
            string city, string zipCode, string country, double latitude, double longitude)
        {
            BuildingName = buildingName;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
            City = city;
            ZipCode = zipCode;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Address Update(string buildingName, string street, string buildingNumber, string apartmentNumber, 
            string city, string zipCode, string country, double? latitude, double? longitude)
        {
            BuildingName = !string.IsNullOrWhiteSpace(buildingName) ? buildingName : BuildingName;
            Street = !string.IsNullOrWhiteSpace(street) ? street : Street;
            BuildingNumber = !string.IsNullOrWhiteSpace(buildingNumber) ? buildingNumber : BuildingNumber;
            ApartmentNumber = !string.IsNullOrWhiteSpace(apartmentNumber) ? apartmentNumber : ApartmentNumber;
            City = !string.IsNullOrWhiteSpace(city) ? city : City;
            ZipCode = !string.IsNullOrWhiteSpace(zipCode) ? zipCode : ZipCode;
            Country = !string.IsNullOrWhiteSpace(country) ? country : Country;
            Latitude = latitude ?? Latitude; 
            Longitude = longitude ?? Longitude; 

            return this;
        }
    }
}
