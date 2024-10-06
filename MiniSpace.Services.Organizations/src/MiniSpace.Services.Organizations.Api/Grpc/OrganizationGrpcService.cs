// using Grpc.Core;
// using MiniSpace.Services.Organizations.Core.Repositories;
// using MiniSpace.Services.Organizations.Grpc;
// using System;
// using System.Linq;
// using System.Threading.Tasks;

// namespace MiniSpace.Services.Organizations.Api.Grpc
// {
//     public class OrganizationGrpcService : OrganizationService.OrganizationServiceBase
//     {
//         private readonly IOrganizationRepository _organizationRepository;

//         public OrganizationGrpcService(IOrganizationRepository organizationRepository)
//         {
//             _organizationRepository = organizationRepository;
//         }

//         public override async Task<OrganizationDto> GetOrganization(GetOrganizationRequest request, ServerCallContext context)
//         {
//             var organization = await _organizationRepository.GetAsync(Guid.Parse(request.OrganizationId));
//             return new OrganizationDto
//             {
//                 Id = organization.Id.ToString(),
//                 Name = organization.Name,
//                 Description = organization.Description,
//                 OwnerId = organization.OwnerId.ToString(),
//                 BannerUrl = organization.BannerUrl,
//                 ImageUrl = organization.ImageUrl
//             };
//         }

//         public override async Task<OrganizationDetailsDto> GetOrganizationDetails(GetOrganizationDetailsRequest request, ServerCallContext context)
//         {
//             var organization = await _organizationRepository.GetAsync(Guid.Parse(request.OrganizationId));
//             var roles = organization.Roles.Select(role => new RoleDto
//             {
//                 Id = role.Id.ToString(),
//                 Name = role.Name
//             }).ToList();

//             return new OrganizationDetailsDto
//             {
//                 Id = organization.Id.ToString(),
//                 Name = organization.Name,
//                 Description = organization.Description,
//                 OwnerId = organization.OwnerId.ToString(),
//                 BannerUrl = organization.BannerUrl,
//                 ImageUrl = organization.ImageUrl,
//                 Roles = { roles }
//             };
//         }

//         public override async Task<OrganizationsList> GetRootOrganizations(EmptyRequest request, ServerCallContext context)
//         {
//             // Assuming GetOrganizerOrganizationsAsync is used to get root organizations
//             // Here we can pass a null or special "root" organizerId to signify root organizations
//             var rootOrganizations = await _organizationRepository.GetOrganizerOrganizationsAsync(Guid.Empty); // Assuming Guid.Empty signifies root organizations
//             var response = new OrganizationsList();
//             response.Organizations.AddRange(rootOrganizations.Select(org => new OrganizationDto
//             {
//                 Id = org.Id.ToString(),
//                 Name = org.Name,
//                 Description = org.Description,
//                 OwnerId = org.OwnerId.ToString(),
//                 BannerUrl = org.BannerUrl,
//                 ImageUrl = org.ImageUrl
//             }));
//             return response;
//         }

//         public override async Task<OrganizationsList> GetChildrenOrganizations(GetChildrenOrganizationsRequest request, ServerCallContext context)
//         {
//             // Using GetOrganizerOrganizationsAsync with the parent organizationId
//             var childrenOrganizations = await _organizationRepository.GetOrganizerOrganizationsAsync(Guid.Parse(request.OrganizationId));
//             var response = new OrganizationsList();
//             response.Organizations.AddRange(childrenOrganizations.Select(org => new OrganizationDto
//             {
//                 Id = org.Id.ToString(),
//                 Name = org.Name,
//                 Description = org.Description,
//                 OwnerId = org.OwnerId.ToString(),
//                 BannerUrl = org.BannerUrl,
//                 ImageUrl = org.ImageUrl
//             }));
//             return response;
//         }

//         public override async Task<GuidsList> GetAllChildrenOrganizations(GetAllChildrenOrganizationsRequest request, ServerCallContext context)
//         {
//             // Assuming GetOrganizerOrganizationsAsync returns all organizations under a parent, we can use the same method
//             var childrenOrganizations = await _organizationRepository.GetOrganizerOrganizationsAsync(Guid.Parse(request.OrganizationId));
//             var response = new GuidsList();
//             response.Ids.AddRange(childrenOrganizations.Select(org => org.Id.ToString()));
//             return response;
//         }
//     }
// }
