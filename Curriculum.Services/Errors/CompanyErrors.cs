using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;

public record CompanyNotFoundError : NotFoundError<Company>
{
    public CompanyNotFoundError(Guid id, IDictionary<string, object?>? httpExtensions = null)
        : base($"Company with id {id} not found.",  httpExtensions)
    { }
    
    public CompanyNotFoundError(string name, IDictionary<string, object?>? httpExtensions = null)
        : base($"Company with name '{name}' not found.",  httpExtensions)
    { }
}