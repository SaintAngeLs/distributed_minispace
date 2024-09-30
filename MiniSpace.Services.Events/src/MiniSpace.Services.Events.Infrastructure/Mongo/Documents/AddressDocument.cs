 using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Paralax.Types;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Infrastructure.Mongo.Documents
{
    [ExcludeFromCodeCoverage]
    public class AddressDocument
    {
        public string BuildingName { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public static AddressDocument FromEntity(Address address)
        {
            return new AddressDocument
            {
                BuildingName = address.BuildingName,
                Street = address.Street,
                BuildingNumber = address.BuildingNumber,
                ApartmentNumber = address.ApartmentNumber,
                City = address.City,
                ZipCode = address.ZipCode,
                Country = address.Country
            };
        }

        public Address ToEntity()
        {
            return new Address(BuildingName, Street, BuildingNumber, ApartmentNumber, City, ZipCode, Country);
        }
    }
}