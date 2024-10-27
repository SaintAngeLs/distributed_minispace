using MiniSpace.Services.Events.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Events.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class AddressDto
    {
        // Properties
        public string BuildingName { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }   
        public double Longitude { get; set; }  

        // Default constructor
        public AddressDto()
        {
        }

        public AddressDto(Address address)
        {
            BuildingName = address.BuildingName;
            Street = address.Street;
            BuildingNumber = address.BuildingNumber;
            ApartmentNumber = address.ApartmentNumber;
            City = address.City;
            ZipCode = address.ZipCode;
            Country = address.Country;
            Latitude = address.Latitude;
            Longitude = address.Longitude;
        }
    }
}
