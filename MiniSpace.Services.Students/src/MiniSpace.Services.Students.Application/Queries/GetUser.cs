using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUser : IQuery<StudentDto>
    {
        public Guid StudentId { get; set; }
    }    
}
