using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Core.Exceptions;

namespace MiniSpace.Services.Students.Core.Entities
{
    public class Student : AggregateRoot
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public int Friends { get; private set; }
        public string ProfileImage { get; private set; }
        public string Description { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public bool EmailNotifications { get; private set; }
        public bool IsOrganizer { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Student(Guid id, string username, string password, string email, DateTime createdAt)
            : this(id, username, password, email, createdAt, 0, string.Empty,
                string.Empty, null, false, false)
        {}

        public Student(Guid id, string username, string password, string email, DateTime createdAt,
            int friends, string profileImage, string description, DateTime? dateOfBirth,
            bool emailNotifications, bool isOrganizer)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            CreatedAt = createdAt;
            Friends = friends;
            ProfileImage = profileImage;
            Description = description;
            DateOfBirth = dateOfBirth;
            EmailNotifications = emailNotifications;
            IsOrganizer = isOrganizer;
        }
    }    
}
