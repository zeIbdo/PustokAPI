using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class InvalidParentIdException(string message = "Parent id cannot be same as id") : AppException(message, HttpStatusCode.BadRequest);