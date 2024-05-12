using System;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Events.Rejected
{
    public class AddReactionRejected(Guid userId, string reason, string code) : IRejectedEvent
    {
        public Guid UserId { get; } = userId;
        public string Reason { get; } = reason;
        public string Code { get; } = code;
    }
}