using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;

namespace MiniSpace.Services.Students.Application.Commands
{
    public class DeleteStudent : ICommand
    {
        public Guid Id;

        public DeleteStudent(Guid id) => Id = id;
    }    
}
