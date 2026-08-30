using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class UnauthorizedException(string message = "You are unauthorized") : AppException(message, HttpStatusCode.Unauthorized);
