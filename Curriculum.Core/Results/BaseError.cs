using System.Net;

namespace Curriculum.Core.Results;

public abstract record BaseError(
    string Message,
    HttpStatusCode HttpStatusCode,
    string HttpTitle,
    IDictionary<string, object?>? HttpExtensions = null
);