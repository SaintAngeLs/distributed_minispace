using System;
using MiniSpace.Services.Organizations.Core.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    [ExcludeFromCodeCoverage]
    public class InvitationDto
    {
        public Guid UserId { get; set; }

        public InvitationDto()
        {
            
        }

        public InvitationDto(Invitation invitation)
        {
            UserId = invitation.UserId;
        }
    }
}
