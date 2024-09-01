using System;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class View
    {
        public Guid UserProfileId { get; private set; }
        public DateTime Date { get; private set; }
        public string IpAddress { get; private set; }
        public string DeviceType { get; private set; }
        public string OperatingSystem { get; private set; }

        public View(Guid userProfileId, DateTime date, string ipAddress, string deviceType, string operatingSystem)
        {
            UserProfileId = userProfileId;
            Date = date;
            IpAddress = ipAddress;
            DeviceType = deviceType;
            OperatingSystem = operatingSystem;
        }
    }
}
