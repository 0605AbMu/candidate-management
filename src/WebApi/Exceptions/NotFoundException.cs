using System.Net;
using CM.WebApi.Exceptions.Common;

namespace CM.WebApi.Exceptions;

public class NotFoundException : ApiException
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public NotFoundException()
    {
    }

    public static T ThrowIsNull<T>(T? value) => value ?? throw new NotFoundException("Resource is not found");
}
