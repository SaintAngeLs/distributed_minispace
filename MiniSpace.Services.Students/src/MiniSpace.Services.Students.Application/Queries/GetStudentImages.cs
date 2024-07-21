using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetStudentImages : IQuery<StudentImagesDto>
    {
        public Guid StudentId { get; set; }

        public GetStudentImages(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
