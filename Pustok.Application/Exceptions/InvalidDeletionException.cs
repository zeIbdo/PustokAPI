using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class InvalidDeletionException : Exception, ICustomException
{
    public InvalidDeletionException(string message="Cannot delete enitity with existing related entities") : base(message)
    {
    }

    public HttpStatusCode StatusCode { get ; set ; } = HttpStatusCode.BadRequest;
}
