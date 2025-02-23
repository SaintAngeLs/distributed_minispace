using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentGalleryOfImagesUpdated : IDomainEvent
    {
        public User User { get; }

        public StudentGalleryOfImagesUpdated(User student)
        {
            User = student;
        }
    }
}
