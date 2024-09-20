using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class DeviceInfo
    {
        public string IpAddress { get; set; }
        public string DeviceType { get; set; }
        public string OperatingSystem { get; set; }
    }
}