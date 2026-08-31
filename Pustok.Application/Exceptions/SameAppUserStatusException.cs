using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class SameAppUserStatusException(string message="Same user status"):AppException(message,HttpStatusCode.BadRequest)
{
}
