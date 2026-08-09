using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record CompanyNotFoundError(
    Guid Id,
    IDictionary<string, object?>? HttpExtensions = null
    ) : NotFoundError<Company>($"Company with id {Id} not found.", HttpExtensions);