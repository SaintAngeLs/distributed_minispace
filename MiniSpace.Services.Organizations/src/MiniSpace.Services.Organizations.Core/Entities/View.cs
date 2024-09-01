using System;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class View
    {
        public Guid OrganizationId { get; private set; }
        public DateTime Date { get; private set; }
        public string IpAddress { get; private set; }
        public string DeviceType { get; private set; }
        public string OperatingSystem { get; private set; }

        public View(Guid organizationId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            OrganizationId = organizationId;
            Date = date;
            IpAddress = ipAddress;
            DeviceType = deviceType;
            OperatingSystem = operatingSystem;
        }
    }
}
