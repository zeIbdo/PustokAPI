using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class AlreadyExistsException : Exception, ICustomException
{
    public AlreadyExistsException(string message = "Already exists") : base(message)
    {
    }

    public HttpStatusCode StatusCode { get ; set ; } = HttpStatusCode.Conflict;
}
