using System.Net;

namespace CM.WebApi.Exceptions.Common;

public abstract class ApiException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }

    public ApiException(string message) : base(message)
    {
    }

    public ApiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ApiException()
    {
    }
}
