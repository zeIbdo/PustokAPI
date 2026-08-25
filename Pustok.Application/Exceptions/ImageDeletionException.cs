using Pustok.Application.Exceptions.Generic;
using System.Net;

namespace Pustok.Application.Exceptions;

internal class ImageDeletionException(string message= "Something wrong happened at image deletion"):AppException(message);