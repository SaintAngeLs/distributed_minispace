using System;
using Convey.Types;
using MiniSpace.Services.Students.Core.Entities;

namespace MiniSpace.Services.Students.Infrastructure.Mongo.Documents
{
    public class UserProfileViewDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }
        public DateTime Date { get; set; }
        public string IpAddress { get; set; }
        public string DeviceType { get; set; }
        public string OperatingSystem { get; set; }

        public static UserProfileViewDocument FromEntity(UserProfileView view)
        {
            return new UserProfileViewDocument
            {
                Id = Guid.NewGuid(),
                UserProfileId = view.UserProfileId,
                Date = view.Date,
                IpAddress = view.IpAddress,
                DeviceType = view.DeviceType,
                OperatingSystem = view.OperatingSystem
            };
        }

        public UserProfileView ToEntity()
        {
            return new UserProfileView(UserProfileId, Date, IpAddress, DeviceType, OperatingSystem);
        }
    }
}