using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

 public class AlreadyExistsException(string message = "Already exists") : AppException(message, HttpStatusCode.BadRequest);

