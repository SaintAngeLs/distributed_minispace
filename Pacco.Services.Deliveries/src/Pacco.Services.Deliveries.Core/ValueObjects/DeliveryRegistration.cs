using System;

namespace Pacco.Services.Deliveries.Core.ValueObjects
{
    public struct DeliveryRegistration : IEquatable<DeliveryRegistration>
    {
        public string Description { get;  }
        public DateTime DateTime { get; }

        public DeliveryRegistration(string description, DateTime dateTime)
        {
            Description = description;
            DateTime = dateTime;
        }

        public override bool Equals(object obj)
        {
            if (obj is DeliveryRegistration other)
            {
                return string.Equals(Description, other.Description) && DateTime.Equals(other.DateTime);
            }

            return false;
        }

        public bool Equals(DeliveryRegistration other)
        {
            return string.Equals(Description, other.Description) && DateTime.Equals(other.DateTime);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Description != null ? Description.GetHashCode() : 0) * 397) ^ DateTime.GetHashCode();
            }
        }
    }
}