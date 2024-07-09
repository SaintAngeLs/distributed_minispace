using MiniSpace.Services.Students.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentGalleryOfImagesUpdated : IDomainEvent
    {
        public Student Student { get; }

        public StudentGalleryOfImagesUpdated(Student student)
        {
            Student = student;
        }
    }
}
