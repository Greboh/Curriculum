using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record ProjectNotFoundError(
    Guid Id,
    IDictionary<string, object?>? HttpExtensions = null
    ) : NotFoundError($"Project with id {Id} not found.", HttpExtensions);