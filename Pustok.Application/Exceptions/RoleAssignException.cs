using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

public class RoleAssignException(string message="Only one admin!!!"):AppException(message,HttpStatusCode.BadRequest)
{
}
