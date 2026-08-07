using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record SkillNotFoundError(
    Guid Id,
    IDictionary<string, object?>? HttpExtensions = null
    ) : NotFoundError($"Skill with id {Id} not found.", HttpExtensions);