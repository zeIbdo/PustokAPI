using System.Net;

namespace Pustok.Application.Exceptions.Generic;

public abstract class AppException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}
