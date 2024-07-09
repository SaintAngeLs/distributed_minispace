using Convey.CQRS.Events;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Events
{
    [ExcludeFromCodeCoverage]
    public class StudentUpdated : IEvent
    {
        public Guid StudentId { get; }
        public string FullName { get; }
        public Guid MediaFileId { get; }
        public Guid BannerMediaFileId { get; }
        public IEnumerable<Guid> GalleryOfImages { get; }
        public string Education { get; }
        public string WorkPosition { get; }
        public string Company { get; }
        public IEnumerable<string> Languages { get; }
        public IEnumerable<string> Interests { get; }

        public StudentUpdated(Guid studentId, string fullName, Guid mediaFileId, Guid bannerMediaFileId,
                              IEnumerable<Guid> galleryOfImages, string education, string workPosition,
                              string company, IEnumerable<string> languages, IEnumerable<string> interests)
        {
            StudentId = studentId;
            FullName = fullName;
            MediaFileId = mediaFileId;
            BannerMediaFileId = bannerMediaFileId;
            GalleryOfImages = galleryOfImages;
            Education = education;
            WorkPosition = workPosition;
            Company = company;
            Languages = languages;
            Interests = interests;
        }
    }    
}

