using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class NotFoundException(string message = "Not found") : AppException(message, HttpStatusCode.NotFound);