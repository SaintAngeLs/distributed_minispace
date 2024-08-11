using System;
using System.Collections.Generic;
using System.Linq;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class OrganizationRequests : AggregateRoot
    {
        public Guid OrganizationId { get; private set; }
        private ISet<OrganizationRequest> _requests = new HashSet<OrganizationRequest>();

        public IEnumerable<OrganizationRequest> Requests
        {
            get => _requests;
            private set => _requests = new HashSet<OrganizationRequest>(value);
        }

        public OrganizationRequests(Guid organizationId, IEnumerable<OrganizationRequest> requests = null)
        {
            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            Requests = requests ?? new HashSet<OrganizationRequest>();
        }

        public void AddRequest(OrganizationRequest request)
        {
            if (_requests.Any(r => r.Id == request.Id))
            {
                throw new InvalidOperationException("A request with the same ID already exists.");
            }

            _requests.Add(request);
            AddEvent(new OrganizationRequestCreated(request.Id, OrganizationId, request.UserId, request.RequestDate));
        }

        public void ApproveRequest(Guid requestId)
        {
            var request = _requests.SingleOrDefault(r => r.Id == requestId);
            if (request == null)
            {
                throw new InvalidOperationException("Request not found.");
            }

            request.Approve();
            AddEvent(new OrganizationRequestApproved(request.Id, OrganizationId, request.UserId, DateTime.UtcNow));
        }

        public void RejectRequest(Guid requestId, string reason)
        {
            var request = _requests.SingleOrDefault(r => r.Id == requestId);
            if (request == null)
            {
                throw new InvalidOperationException("Request not found.");
            }

            request.Reject(reason);
            AddEvent(new OrganizationRequestRejected(request.Id, OrganizationId, request.UserId, DateTime.UtcNow, reason));
        }

        public void CancelRequest(Guid requestId)
        {
            var request = _requests.SingleOrDefault(r => r.Id == requestId);
            if (request == null)
            {
                throw new InvalidOperationException("Request not found.");
            }

            request.Cancel();
            AddEvent(new OrganizationRequestCanceled(request.Id, OrganizationId, request.UserId, DateTime.UtcNow));
        }

        public void UpdateRequestReason(Guid requestId, string newReason)
        {
            var request = _requests.SingleOrDefault(r => r.Id == requestId);
            if (request == null)
            {
                throw new InvalidOperationException("Request not found.");
            }

            request.UpdateReason(newReason);
            AddEvent(new OrganizationRequestUpdated(request.Id, OrganizationId, request.UserId, DateTime.UtcNow, newReason));
        }
    }
}
