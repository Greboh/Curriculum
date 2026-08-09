using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface ISkillService
{
    IReadOnlyList<Skill> GetAll();
    Result<Skill> GetById(Guid id);
    Result<Skill> Create(string name);
    Result<bool> Delete(string name);
}

public class SkillService(ICurriculumData data) : ISkillService
{
    public IReadOnlyList<Skill> GetAll()
        => data.Skills;

    public Result<Skill> GetById(Guid id)
    {
        var skill = data.Skills
            .FirstOrDefault(x => x.Id == id);

        if (skill is null)
        {
            return new SkillNotFoundError(id);
        }

        return skill;
    }

    public Result<Skill> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new SkillValidationError(
                name,
                new Dictionary<string, object>
                {
                    { "Name", "Is Null or Empty" }
                }
            );
        }

        var skill = new Skill
        {
            Id = Guid.CreateVersion7(),
            Name = name.Trim()
        };
        
        return data.CreateSkill(skill);
    }

    public Result<bool> Delete(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new SkillValidationError(
                name,
                new Dictionary<string, object>
                {
                    { "Name", "Is Null or Empty" }
                }
            );
        }

        return data.DeleteSkill(name);
    }
}