using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSpace.APIGateway.Ocelot.Infrastructure
{
    public interface IAnonymousRouteValidator
    {
        bool HasAccess(string path);
    }
}