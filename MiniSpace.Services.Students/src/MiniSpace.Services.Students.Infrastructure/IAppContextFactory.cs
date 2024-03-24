using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniSpace.Services.Students.Application;

namespace MiniSpace.Services.Students.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}
