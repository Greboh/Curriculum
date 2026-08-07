using Curriculum.Core.Entities;
using Curriculum.Core.Results;
using Curriculum.Infrastructure.Persistence;
using Curriculum.Services.Errors;

namespace Curriculum.Services;

public interface ICompanyService
{
    IReadOnlyList<Company> GetAll();
    Result<Company> GetById(Guid id);
}

public class CompanyService(ICurriculumData data) : ICompanyService
{
    public IReadOnlyList<Company> GetAll()
        => data.Companies;

    public Result<Company> GetById(Guid id)
    {
        var company = data.Companies
            .FirstOrDefault(x => x.Id == id);

        if (company is null)
        {
            return new CompanyNotFoundError(id);
        }

        return company;
    }
}