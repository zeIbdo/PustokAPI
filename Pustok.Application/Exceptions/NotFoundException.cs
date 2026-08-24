using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class NotFoundException : Exception, ICustomException
{
    public NotFoundException(string message = "Not found") : base(message)
    {
    }

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;
}
