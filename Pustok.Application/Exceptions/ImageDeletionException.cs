using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

internal class ImageDeletionException : Exception, ICustomException
{
    public ImageDeletionException(string message = "Something wrong happened at image deletion") : base(message)
    {
    }

    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
}
