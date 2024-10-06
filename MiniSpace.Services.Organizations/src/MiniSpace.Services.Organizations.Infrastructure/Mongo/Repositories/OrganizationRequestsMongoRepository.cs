using Paralax.Persistence.MongoDB;
using MiniSpace.Services.Organizations.Core.Entities;
using MiniSpace.Services.Organizations.Core.Repositories;
using MiniSpace.Services.Organizations.Infrastructure.Mongo.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Organizations.Infrastructure.Mongo.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OrganizationRequestsMongoRepository : IOrganizationRequestsRepository
    {
        private readonly IMongoRepository<OrganizationRequestsDocument, Guid> _requestsRepository;

        public OrganizationRequestsMongoRepository(IMongoRepository<OrganizationRequestsDocument, Guid> requestsRepository)
        {
            _requestsRepository = requestsRepository;
        }

        public async Task<OrganizationRequest> GetRequestAsync(Guid organizationId, Guid requestId)
        {
            var requestsDocument = await _requestsRepository.GetAsync(r => r.OrganizationId == organizationId);
            return requestsDocument?.Requests.FirstOrDefault(r => r.RequestId == requestId)?.AsEntity();
        }

        public async Task<IEnumerable<OrganizationRequest>> GetRequestsAsync(Guid organizationId)
        {
            var requestsDocument = await _requestsRepository.GetAsync(r => r.OrganizationId == organizationId);
            return requestsDocument?.Requests.Select(r => r.AsEntity()) ?? Enumerable.Empty<OrganizationRequest>();
        }

        public async Task AddRequestAsync(Guid organizationId, OrganizationRequest request)
        {
            var requestsDocument = await _requestsRepository.GetAsync(r => r.OrganizationId == organizationId);
            if (requestsDocument != null)
            {
                var requests = requestsDocument.Requests.ToList();
                requests.Add(request.AsDocument());
                requestsDocument.Requests = requests;
                await _requestsRepository.UpdateAsync(requestsDocument);
            }
            else
            {
                requestsDocument = new OrganizationRequestsDocument
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = organizationId,
                    Requests = new List<RequestDocument> { request.AsDocument() }
                };
                await _requestsRepository.AddAsync(requestsDocument);
            }
        }

        public async Task UpdateRequestAsync(Guid organizationId, OrganizationRequest request)
        {
            var requestsDocument = await _requestsRepository.GetAsync(r => r.OrganizationId == organizationId);
            if (requestsDocument != null)
            {
                var requests = requestsDocument.Requests.ToList();
                var existingRequest = requests.FirstOrDefault(r => r.RequestId == request.Id);
                if (existingRequest != null)
                {
                    requests.Remove(existingRequest);
                    requests.Add(request.AsDocument());
                    requestsDocument.Requests = requests;
                    await _requestsRepository.UpdateAsync(requestsDocument);
                }
            }
        }

        public async Task DeleteRequestAsync(Guid organizationId, Guid requestId)
        {
            var requestsDocument = await _requestsRepository.GetAsync(r => r.OrganizationId == organizationId);
            if (requestsDocument != null)
            {
                var requests = requestsDocument.Requests.ToList();
                var existingRequest = requests.FirstOrDefault(r => r.RequestId == requestId);
                if (existingRequest != null)
                {
                    requests.Remove(existingRequest);
                    requestsDocument.Requests = requests;
                    await _requestsRepository.UpdateAsync(requestsDocument);
                }
            }
        }
    }
}
