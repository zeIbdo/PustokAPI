using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class InvalidDeletionException(string message= "Cannot delete enitity with existing related entities"):AppException(message,HttpStatusCode.BadRequest);