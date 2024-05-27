using System;
using System.Diagnostics.CodeAnalysis;
using Convey.CQRS.Queries;
using MiniSpace.Services.Identity.Application.DTO;

namespace MiniSpace.Services.Identity.Application.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetUser : IQuery<UserDto>
    {
        public Guid UserId { get; set; }
    }
}