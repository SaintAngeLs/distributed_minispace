using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Web.DTO.Views
{
    public class UserProfileViewDto
    {
        public Guid UserProfileId { get; set; }
        public DateTime Date { get; set; }
        public string IpAddress { get; set; }
        public string DeviceType { get; set; }
        public string OperatingSystem { get; set; }
    }
}