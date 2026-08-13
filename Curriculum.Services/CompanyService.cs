using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;
using Microsoft.EntityFrameworkCore;

namespace Curriculum.Services;

public interface ICompanyService
{
    Task<IReadOnlyList<Company>> GetAll(CancellationToken ct = default);
    Task<Result<Company>> Get(Guid? id, string? name, CancellationToken ct = default);
}

public class CompanyService(CurriculumContext context) : ICompanyService
{
    public async Task<IReadOnlyList<Company>> GetAll(CancellationToken ct = default)
        => await context.Companies.
            AsNoTracking()
            .ToListAsync(ct);

    public async Task<Result<Company>> Get(Guid? id, string? name, CancellationToken ct = default)
    {
        Company? company;
        
        if (id.HasValue)
        {
            company = await context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id.Value, ct);
            
            return company is null
                ? new CompanyNotFoundError(id.Value)
                : company;
        }

        var trimmedName = name!.Trim();
        company = await context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == trimmedName, ct);
        
        return company is null
            ? new CompanyNotFoundError(trimmedName)
            : company;
    }
}