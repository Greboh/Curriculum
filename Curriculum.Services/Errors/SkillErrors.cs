using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record SkillNotFoundError(
    Guid Id,
    IDictionary<string, object?>? HttpExtensions = null
    ) : NotFoundError<Skill>($"Skill with id {Id} not found.", HttpExtensions);

public record SkillValidationError(
    string Name,
    IDictionary<string, object> HttpExtensions
) : ValidationError<Skill>($"{Name} failed validation. See Extensions for details.", HttpExtensions);