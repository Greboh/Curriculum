using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public sealed record SkillNotFoundError : NotFoundError<Skill>
{
    public SkillNotFoundError(Guid id, IDictionary<string, object?>? httpExtensions = null)
        : base($"Skill with id {id} not found.", httpExtensions)
    {
    }
    public SkillNotFoundError(string name, IDictionary<string, object?>? httpExtensions = null)
        : base($"Skill with name '{name}' not found.", httpExtensions)
    {
    }
}

public sealed record SkillValidationError(
    string Name,
    IDictionary<string, object> HttpExtensions
) : ValidationError<Skill>($"{Name} failed validation. See Extensions for details.", HttpExtensions);