using System.Net;

namespace Curriculum.Core.Results;

public record NotFoundError<T>(
    string Message,
    IDictionary<string, object?>? HttpExtensions = null
) : BaseError(Message, HttpStatusCode.NotFound, $"Failed to find {typeof(T).Name}", HttpExtensions);

