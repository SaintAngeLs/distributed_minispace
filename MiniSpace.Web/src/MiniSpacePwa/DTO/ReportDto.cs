using System;
using MiniSpacePwa.DTO.Enums;
using MiniSpacePwa.DTO.States;
using MiniSpacePwa.DTO.Types;

namespace MiniSpacePwa.DTO
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