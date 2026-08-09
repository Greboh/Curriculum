using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record ProjectNotFoundError(
    Guid Id,
    IDictionary<string, object?>? HttpExtensions = null
    ) : NotFoundError<Project>($"Project with id {Id} not found.", HttpExtensions);