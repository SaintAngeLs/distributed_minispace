using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace MiniSpace.Services.Students.Application.Events
{
    public class StudentDeleted : IEvent
    {
        public Guid StudentId { get; }

        public StudentDeleted(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}
