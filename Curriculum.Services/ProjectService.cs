using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface IProjectService
{
    IReadOnlyList<Project> GetAll();
    Result<Project> Get(Guid? id,  string? name);
}

public class ProjectService(ICurriculumData data) : IProjectService
{
    public IReadOnlyList<Project> GetAll()
        => data.Projects;

    public Result<Project> Get(Guid? id, string? name)
    {
        Project? project;
        
        if (id.HasValue)
        {
            project = data.Projects
                .FirstOrDefault(x => x.Id == id.Value);
            
            return project is null
                ? new ProjectNotFoundError(id.Value)
                : project;
        }

        project = data.Projects
            .FirstOrDefault(x => x.Name == name?.Trim());
        
        return project is null
            ? new ProjectNotFoundError(name!)
            : project;
    }
}