using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Core.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidStudentProfileImageException : DomainException
    {
        public override string Code { get; } = "invalid_student_profile_image";
        public Guid Id { get; }
        public string ProfileImage { get; }
        
        public InvalidStudentProfileImageException(Guid id, string profileImage) : base(
            $"Student with id: {id} has invalid profile image.")
        {
            Id = id;
            ProfileImage = profileImage;
        }
    }
}
