using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.Services.Students.Application.Exceptions
{
    public class AppException : Exception
    {
        public virtual string Code { get; }
        
        protected AppException(string message) : base(message)
        {
        }
    }
}
