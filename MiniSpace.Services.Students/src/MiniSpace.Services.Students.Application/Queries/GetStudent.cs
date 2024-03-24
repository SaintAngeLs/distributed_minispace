using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using MiniSpace.Services.Students.Application.Dto;

namespace MiniSpace.Services.Students.Application.Queries
{
    public class GetStudent : IQuery<CustomerDto>
    {
        public Guid StudentId { get; set; }
    }    
}
