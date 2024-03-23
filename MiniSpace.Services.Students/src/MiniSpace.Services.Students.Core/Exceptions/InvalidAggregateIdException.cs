using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Core.Exceptions
{
    public class InvalidAggregateIdException : DomainException
    {
        public override string Code { get; } = "invalid_aggregate_id";
        
        public InvalidAggregateIdException() : base($"Invalid aggregate id.")
        {
        }
    }
}
