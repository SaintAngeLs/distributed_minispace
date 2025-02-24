using Paralax.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Students.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUserImages : IQuery<UserImagesDto>
    {
        public Guid UserId { get; set; }

        public GetUserImages(Guid userId)
        {
            UserId = userId;
        }
    }
}
