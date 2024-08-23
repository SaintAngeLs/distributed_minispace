using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Convey.WebApi.Exceptions;

namespace MiniSpace.APIGateway.Ocelot.Infrastructure
{
    public class ExceptionToResponseMapper  : IExceptionToResponseMapper
    {
          public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                _ => new ExceptionResponse(new {code = "error", reason = "There was an error."},
                    HttpStatusCode.BadRequest)
            };
        
    }
}