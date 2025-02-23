using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class UserGalleryOfImagesUpdated : IDomainEvent
    {
        public User User { get; }

        public UserGalleryOfImagesUpdated(User student)
        {
            User = student;
        }
    }
}
