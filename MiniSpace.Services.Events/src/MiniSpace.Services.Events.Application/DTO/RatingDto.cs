using System;

namespace MiniSpace.Services.Events.Application.DTO
{
    public class RatingDto
    {
        public Guid StudentId { get; set; }
        public int Value { get; set; }

        public RatingDto()
        {
        }

        public RatingDto(Guid studentId, int value)
        {
            StudentId = studentId;
            Value = value;
        }
    }
}
