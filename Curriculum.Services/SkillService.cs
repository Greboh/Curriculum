using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface ISkillService
{
    IReadOnlyList<Skill> GetAll();
    Result<Skill> GetById(Guid id);
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
}