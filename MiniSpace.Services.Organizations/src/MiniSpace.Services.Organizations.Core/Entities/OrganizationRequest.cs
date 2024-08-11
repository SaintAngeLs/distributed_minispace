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

        // Factory method to create a new request
        public static OrganizationRequest CreateNew(Guid userId, string reason)
        {
            return new OrganizationRequest(Guid.NewGuid(), userId, DateTime.UtcNow, RequestState.Pending, reason);
        }

        // Factory method to create an existing request (e.g., when loading from a database)
        public static OrganizationRequest CreateExisting(Guid requestId, Guid userId, DateTime requestDate, RequestState state, string reason)
        {
            return new OrganizationRequest(requestId, userId, requestDate, state, reason);
        }

        private OrganizationRequest(Guid requestId, Guid userId, DateTime requestDate, RequestState state, string reason)
        {
            Id = requestId;
            UserId = userId;
            RequestDate = requestDate;
            State = state;
            Reason = reason;
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
