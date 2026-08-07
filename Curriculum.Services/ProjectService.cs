using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface IProjectService
{
    IReadOnlyList<Project> GetAll();
    Result<Project> GetById(Guid id);
}

public class ProjectService(ICurriculumData data) : IProjectService
{
    public IReadOnlyList<Project> GetAll()
        => data.Projects;

    public Result<Project> GetById(Guid id)
    {
        var project = data.Projects
            .FirstOrDefault(x => x.Id == id);

        if (project == null)
        {
            return new ProjectNotFoundError(id);
        }

        return project;
    }
}