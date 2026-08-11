using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface ICompanyService
{
    IReadOnlyList<Company> GetAll();
    Result<Company> Get(Guid? id, string? name);
}

public class CompanyService(ICurriculumData data) : ICompanyService
{
    public IReadOnlyList<Company> GetAll()
        => data.Companies;

    public Result<Company> Get(Guid? id, string? name)
    {
        Company? company;
        
        if (id.HasValue)
        {
            company = data.Companies
                .FirstOrDefault(x => x.Id == id.Value);
            
            return company is null
                ? new CompanyNotFoundError(id.Value)
                : company;
        }

        company = data.Companies
            .FirstOrDefault(x => x.Name == name?.Trim());
        
        return company is null
            ? new CompanyNotFoundError(name!)
            : company;
    }
}