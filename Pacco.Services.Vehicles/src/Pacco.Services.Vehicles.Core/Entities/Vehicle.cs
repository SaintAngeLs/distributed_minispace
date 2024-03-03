using System;
using Pacco.Services.Vehicles.Core.Exceptions;

namespace Pacco.Services.Vehicles.Core.Entities
{
    public class Vehicle
    {
        public Guid Id { get; protected set; }
        public string Brand { get; protected set; }
        public string Model { get; protected set; }
        public string Description { get; protected set; }
        public double PayloadCapacity { get; protected set; }
        public double LoadingCapacity { get; protected set; }
        public decimal PricePerService { get; protected set; }
        public Variants Variants { get; protected set; }

        public Vehicle(Guid id, string brand, string model, string description, double payloadCapacity,
            double loadingCapacity, decimal pricePerService)
        {
            Id = id;
            Brand = brand;
            Model = model;
            ChangeDescription(description);
            PayloadCapacity = payloadCapacity > 0 ? payloadCapacity : throw new InvalidVehicleCapacity(payloadCapacity);
            LoadingCapacity = loadingCapacity > 0 ? loadingCapacity : throw new InvalidVehicleCapacity(loadingCapacity);
            ChangePricePerService(pricePerService);
            AddVariants(Variants.Standard);
        }
        
        public Vehicle(Guid id, string brand, string model, string description, double payloadCapacity,
            double loadingCapacity, decimal pricePerService, params Variants[] variants) 
            : this(id, brand, model, description, payloadCapacity, loadingCapacity, pricePerService)
        {
            AddVariants(variants);
        }

        public void ChangeDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new InvalidVehicleDescriptionException(description);
            }

            Description = description;
        }

        public void ChangePricePerService(decimal pricePerService)
        {
            if (pricePerService <= 0)
            {
                throw  new InvalidVehiclePricePerServiceException(pricePerService);
            }

            PricePerService = pricePerService;
        }

        public void ChangeVariants(Variants variants)
            => Variants = variants;

        public void AddVariants(params Variants[] variants)
        {
            foreach (var variant in variants)
            {
                Variants |= variant;
            }
        }
        
        public void RemoveVariants(params Variants[] variants)
        {
            foreach (var variant in variants)
            {
                Variants &= ~variant;
            }
        }
    }
}