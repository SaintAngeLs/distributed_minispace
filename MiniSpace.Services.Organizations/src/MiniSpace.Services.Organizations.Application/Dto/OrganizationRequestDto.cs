using System;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    public class OrganizationRequestDto
    {
        public Guid RequestId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string State { get; set; }
        public string Reason { get; set; }

        // Factory method to create a DTO from an OrganizationRequest entity
        public static OrganizationRequestDto FromEntity(OrganizationRequest request)
        {
            return new OrganizationRequestDto
            {
                RequestId = request.Id,
                UserId = request.UserId,
                RequestDate = request.RequestDate,
                State = request.State.ToString(),
                Reason = request.Reason
            };
        }
    }
}
