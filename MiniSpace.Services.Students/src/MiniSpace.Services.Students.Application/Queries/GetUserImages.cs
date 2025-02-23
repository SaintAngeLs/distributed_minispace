using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserImages : IQuery<StudentImagesDto>
    {
        public Guid StudentId { get; set; }

        public GetUserImages(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
