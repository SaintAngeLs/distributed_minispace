using System;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniSpace.Services.Events.Core.Entities;

namespace MiniSpace.Services.Events.Application.Events.Rejected
{
    public class AddEventRejected(Guid userId, string reason, string code) : IRejectedEvent
    {
        public Guid UserId { get; } = userId;
        public string Reason { get; } = reason;
        public string Code { get; } = code;
    }
}