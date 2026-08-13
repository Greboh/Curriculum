using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;
using Microsoft.EntityFrameworkCore;

namespace Curriculum.Services;

public interface IEducationService
{
    Task<IReadOnlyList<Education>> GetAll(CancellationToken ct = default);
    Task<Result<Education>> Get(Guid? id, string? institution, CancellationToken ct = default);
}

public class EducationService(CurriculumContext context) : IEducationService
{
    public async Task<IReadOnlyList<Education>> GetAll(CancellationToken ct = default)
        => await context.Educations
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Result<Education>> Get(Guid? id, string? institution, CancellationToken ct = default)
    {
        Education? education;

        if (id.HasValue)
        {
            education = await context.Educations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id.Value, ct);

            return education is null
                ? new EducationNotFoundError(id.Value)
                : education;
        }

        var trimmedInstitution = institution!.Trim();
        education = await context.Educations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Institution == trimmedInstitution, ct);

        return education is null
            ? new EducationNotFoundError(trimmedInstitution)
            : education;
    }
}