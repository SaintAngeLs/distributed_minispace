using System;
using System.Diagnostics.CodeAnalysis;
using Paralax.CQRS.Events;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniSpace.Services.Reactions.Core.Entities;

namespace MiniSpace.Services.Reactions.Application.Events.Rejected
{
    [ExcludeFromCodeCoverage]
    public class DeleteReactionRejected(Guid userId, string reason, string code) : IRejectedEvent
    {
        public Guid UserId { get; } = userId;
        public string Reason { get; } = reason;
        public string Code { get; } = code;
    }
}