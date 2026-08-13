using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;
using Microsoft.EntityFrameworkCore;

namespace Curriculum.Services;

public interface IProjectService
{
    Task<IReadOnlyList<Project>> GetAll(CancellationToken ct = default);
    Task<Result<Project>> Get(Guid? id, string? name, CancellationToken ct = default);
}

public class ProjectService(CurriculumContext context) : IProjectService
{
    public async Task<IReadOnlyList<Project>> GetAll(CancellationToken ct = default)
        => await context.Projects
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Result<Project>> Get(Guid? id, string? name, CancellationToken ct = default)
    {
        Project? project;

        if (id.HasValue)
        {
            project = await context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id.Value, ct);

            return project is null
                ? new ProjectNotFoundError(id.Value)
                : project;
        }

        var trimmedName = name!.Trim();
        project = await context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == trimmedName, ct);

        return project is null
            ? new ProjectNotFoundError(name!)
            : project;
    }
}