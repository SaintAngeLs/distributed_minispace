using System;
using System.Collections.Generic;

namespace MiniSpace.Services.Notifications.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberOfFriends { get; set; }
        public Guid ProfileImage { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool EmailNotifications { get; set; }
        public bool IsBanned { get; set; }
        public bool IsOrganizer { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Guid> InterestedInEvents { get; set; }
        public List<Guid> SignedUpEvents { get; set; }

        public User(Guid id, string email, string firstName, string lastName, 
            int numberOfFriends, Guid profileImage, string description, 
            DateTime dateOfBirth, bool emailNotifications, bool isBanned,
            bool isOrganizer, string state, DateTime createdAt)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            NumberOfFriends = numberOfFriends;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
            IsBanned = isBanned;
            IsOrganizer = isOrganizer;
            State = state;
            CreatedAt = createdAt;
        }
    }
}
