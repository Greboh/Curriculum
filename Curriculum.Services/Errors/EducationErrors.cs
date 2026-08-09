using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record EducationNotFoundError(
    Guid Id,
    IDictionary<string, object?>? HttpExtensions = null
    ) : NotFoundError<Education>($"Education with id {Id} not found.", HttpExtensions);