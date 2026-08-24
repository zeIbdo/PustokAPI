using System.Net;

namespace Pustok.Application.Exceptions.Generic;

public interface ICustomException
{
    public HttpStatusCode StatusCode { get; set; }
}
