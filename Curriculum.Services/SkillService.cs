using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;
using Microsoft.EntityFrameworkCore;

namespace Curriculum.Services;

public interface ISkillService
{
    Task<IReadOnlyList<Skill>> GetAll(CancellationToken ct = default);

    Task<Result<Skill>> Get(
        Guid? id,
        string? name,
        CancellationToken ct = default
    );

    Task<Result<Skill>> Create(string name, CancellationToken ct = default);

    Task<bool> Delete(Guid? id,
        string? name,
        CancellationToken ct = default
    );
}

public class SkillService(CurriculumContext context) : ISkillService
{
    public async Task<IReadOnlyList<Skill>> GetAll(CancellationToken ct = default)
        => await context.Skills
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Result<Skill>> Get(
        Guid? id,
        string? name,
        CancellationToken ct = default
    )
    {
        Skill? skill;

        if (id.HasValue)
        {
            skill = await context.Skills
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id.Value, ct);

            return skill is null
                ? new SkillNotFoundError(id.Value)
                : skill;
        }

        var trimmedName = name!.Trim();
        skill = await context.Skills
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == trimmedName, ct);

        return skill is null
            ? new SkillNotFoundError(trimmedName)
            : skill;
    }

    public async Task<Result<Skill>> Create(string name, CancellationToken ct = default)
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

        context.Skills.Add(skill);
        await context.SaveChangesAsync(ct);

        return skill;
    }

    public async Task<bool> Delete(Guid? id, string? name, CancellationToken ct = default)
    {
        int rowsDeleted;

        if (id.HasValue)
        {
            rowsDeleted = await context.Skills
                .Where(x => x.Id == id.Value)
                .ExecuteDeleteAsync(ct);
        }
        else
        {
            rowsDeleted = await context.Skills
                .Where(x => x.Name == name)
                .ExecuteDeleteAsync(ct);
        }

        return rowsDeleted > 0;
    }
}