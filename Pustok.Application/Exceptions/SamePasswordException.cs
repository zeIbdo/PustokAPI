using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class SamePasswordException(string message = "You cannot use previous password"):AppException(message,HttpStatusCode.BadRequest)
{
}
