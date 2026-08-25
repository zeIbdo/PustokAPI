using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class InvalidParentIdException : Exception, ICustomException
{
    public InvalidParentIdException(string message="Parent id cannot be same as id") : base(message)
    {
    }

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
}
