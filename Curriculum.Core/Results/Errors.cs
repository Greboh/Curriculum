using System.Net;

namespace Curriculum.Core.Results;

public record NotFoundError(
    string Message,
    IDictionary<string, object?>? HttpExtensions = null
) : BaseError(Message, HttpStatusCode.NotFound, "Not Found", HttpExtensions);

