using System;
using Astravent.Web.Wasm.DTO.Enums;
using Astravent.Web.Wasm.DTO.States;
using Astravent.Web.Wasm.DTO.Types;

namespace Astravent.Web.Wasm.DTO
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public Guid IssuerId { get; set; }
        public Guid TargetId { get; set; }
        public Guid TargetOwnerId { get; set; }
        public ReportContextType ContextType { get; set; }
        public ReportCategory Category { get; set; }
        public string Reason { get; set; }
        public ReportState State { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? ReviewerId { get; set; }
    }
}