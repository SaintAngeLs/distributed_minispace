using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Dto
{
    [ExcludeFromCodeCoverage]
    public class StudentWithGalleryImagesDto
    {
        public StudentDto Student { get; set; }
        public List<GalleryImageDto> GalleryImages { get; set; }
    }
}
