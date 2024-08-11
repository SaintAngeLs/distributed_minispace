using System;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class OrganizationRequest : AggregateRoot
    {
        public Guid OrganizationId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime RequestDate { get; private set; }
        public RequestState State { get; private set; }
        public string Reason { get; private set; }

        public OrganizationRequest(Guid organizationId, Guid userId, string reason)
        {
            Id = Guid.NewGuid();
            OrganizationId = organizationId;
            UserId = userId;
            RequestDate = DateTime.UtcNow;
            State = RequestState.Pending; 
            Reason = reason;

            AddEvent(new OrganizationRequestCreated(Id, OrganizationId, UserId, RequestDate));
        }

        public void Approve()
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be approved.");
            }

            State = RequestState.Approved;
            AddEvent(new OrganizationRequestApproved(Id, OrganizationId, UserId, DateTime.UtcNow));
        }

        public void Reject(string rejectionReason)
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be rejected.");
            }

            State = RequestState.Rejected;
            Reason = rejectionReason;
            AddEvent(new OrganizationRequestRejected(Id, OrganizationId, UserId, DateTime.UtcNow, rejectionReason));
        }

        public void Cancel()
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be canceled.");
            }

            State = RequestState.Canceled;
            AddEvent(new OrganizationRequestCanceled(Id, OrganizationId, UserId, DateTime.UtcNow));
        }

        public void UpdateReason(string newReason)
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Cannot update the reason for a request that is not pending.");
            }

            Reason = newReason;
            AddEvent(new OrganizationRequestUpdated(Id, OrganizationId, UserId, DateTime.UtcNow, newReason));
        }
    }

}