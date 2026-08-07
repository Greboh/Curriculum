using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface IEducationService
{
    IReadOnlyList<Education> GetAll();
    Result<Education> GetById(Guid id);
}

public class EducationService(ICurriculumData data) : IEducationService
{
    public IReadOnlyList<Education> GetAll()
        => data.Educations;

    public Result<Education> GetById(Guid id)
    {
        var education = data.Educations
            .FirstOrDefault(x => x.Id == id);

        if (education == null)
        {
            return new EducationNotFoundError(id);
        }

        return education;
    }
}