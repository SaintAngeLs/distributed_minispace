using System;
using System.Collections.Generic;
using MiniSpace.Services.Organizations.Core.Entities;

namespace MiniSpace.Services.Organizations.Application.DTO
{
    public class OrganizationRequestsDto
    {
        public Guid OrganizationId { get; set; }
        public IEnumerable<OrganizationRequestDto> Requests { get; set; }

        public OrganizationRequestsDto()
        {
            Requests = new List<OrganizationRequestDto>();
        }

        public static OrganizationRequestsDto FromEntity(Guid organizationId, IEnumerable<OrganizationRequest> requests)
        {
            var requestDtos = new List<OrganizationRequestDto>();
            foreach (var request in requests)
            {
                requestDtos.Add(OrganizationRequestDto.FromEntity(request));
            }

            return new OrganizationRequestsDto
            {
                OrganizationId = organizationId,
                Requests = requestDtos
            };
        }
    }
}
