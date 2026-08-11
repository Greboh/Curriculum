using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface ISkillService
{
    IReadOnlyList<Skill> GetAll();
    Result<Skill> Get(Guid? id, string? name);
    Result<Skill> Create(string name);
    Result<Skill> Delete(Guid? id, string? name);
}

public class SkillService(ICurriculumData data) : ISkillService
{
    public IReadOnlyList<Skill> GetAll()
        => data.Skills;

    public Result<Skill> Get(Guid? id, string? name)
    {
        Skill? skill;
        
        if (id.HasValue)
        {
            skill = data.Skills
                .FirstOrDefault(x => x.Id == id.Value);
            
            return skill is null
                ? new SkillNotFoundError(id.Value)
                : skill;
        }

        skill = data.Skills
            .FirstOrDefault(x => x.Name == name?.Trim());
        
        return skill is null
            ? new SkillNotFoundError(name!)
            : skill;
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

    public Result<Skill> Delete(Guid? id, string? name)
    {
        var deletedSkill = data.DeleteSkill(id, name);
        if (deletedSkill is null)
        {
            return id.HasValue
                ? new SkillNotFoundError(id.Value)
                : new SkillNotFoundError(name!);
        }

        return deletedSkill;
    }
}