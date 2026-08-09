using System.Net;

namespace Curriculum.Core.Results;

public record NotFoundError<T>(
    string Message,
    IDictionary<string, object?>? HttpExtensions = null
) : BaseError(Message, HttpStatusCode.NotFound, $"Failed to find {typeof(T).Name}", HttpExtensions);

public record ConflictError<T>(
    string Message,
    IDictionary<string, object?>? HttpExtensions = null
) : BaseError(Message, HttpStatusCode.Conflict, $"Failed to create {typeof(T).Name}", HttpExtensions);

public record ValidationError<T>(
    string Message,
    IDictionary<string, object> HttpExtensions
) : BaseError(Message, HttpStatusCode.BadRequest, $"Validation failed for {typeof(T).Name}", HttpExtensions!);