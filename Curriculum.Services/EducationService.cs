using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface IEducationService
{
    IReadOnlyList<Education> GetAll();
    Result<Education> Get(Guid? id, string? institution);
}

public class EducationService(ICurriculumData data) : IEducationService
{
    public IReadOnlyList<Education> GetAll()
        => data.Educations;

    public Result<Education> Get(Guid? id, string? institution)
    {
        Education? education;
        
        if (id.HasValue)
        {
            education = data.Educations
                .FirstOrDefault(x => x.Id == id.Value);
            
            return education is null
                ? new EducationNotFoundError(id.Value)
                : education;
        }

        education = data.Educations
            .FirstOrDefault(x => x.Institution == institution?.Trim());
        
        return education is null
            ? new EducationNotFoundError(institution!)
            : education;
    }
}