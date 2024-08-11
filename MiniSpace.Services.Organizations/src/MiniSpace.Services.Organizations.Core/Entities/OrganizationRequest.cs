using System;
using MiniSpace.Services.Organizations.Core.Events;

namespace MiniSpace.Services.Organizations.Core.Entities
{
    public class OrganizationRequest : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public DateTime RequestDate { get; private set; }
        public RequestState State { get; private set; }
        public string Reason { get; private set; }

        public OrganizationRequest(Guid userId, string reason)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            RequestDate = DateTime.UtcNow;
            State = RequestState.Pending; 
            Reason = reason;

            // Note: The OrganizationId is not passed here, it's handled by the OrganizationRequests class.
        }

        public void Approve()
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be approved.");
            }

            State = RequestState.Approved;
        }

        public void Reject(string rejectionReason)
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be rejected.");
            }

            State = RequestState.Rejected;
            Reason = rejectionReason;
        }

        public void Cancel()
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Only pending requests can be canceled.");
            }

            State = RequestState.Canceled;
        }

        public void UpdateReason(string newReason)
        {
            if (State != RequestState.Pending)
            {
                throw new InvalidOperationException("Cannot update the reason for a request that is not pending.");
            }

            Reason = newReason;
        }
    }
}
