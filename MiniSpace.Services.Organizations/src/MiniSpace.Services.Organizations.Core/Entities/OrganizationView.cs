using System;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class OrganizationView
    {
        public Guid UserId { get; private set; }
        public DateTime Date { get; private set; }
        public string IpAddress { get; private set; }
        public string DeviceType { get; private set; }
        public string OperatingSystem { get; private set; }

        public OrganizationView(Guid userId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            UserId = userId;
            Date = date;
            IpAddress = ipAddress;
            DeviceType = deviceType;
            OperatingSystem = operatingSystem;
        }
    }
}
