using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record CompanyNotFoundError(
    Guid Id,
    IDictionary<string, object?>? HttpExtensions = null
    ) : NotFoundError($"Company with id {Id} not found.", HttpExtensions);