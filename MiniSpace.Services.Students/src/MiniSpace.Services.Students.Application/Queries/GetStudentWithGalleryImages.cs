using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetStudentWithGalleryImages : IQuery<StudentWithGalleryImagesDto>
    {
        public Guid StudentId { get; set; }
    }
}
