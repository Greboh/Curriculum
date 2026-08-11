using Curriculum.Core.Entities;
using Curriculum.Core.Results;

namespace Curriculum.Services.Errors;
    
public sealed record ProjectNotFoundError : NotFoundError<Project>
{
    public ProjectNotFoundError(Guid id, IDictionary<string, object?>? httpExtensions = null)
        : base($"Project with id {id} not found.",  httpExtensions)
    { }
    
    public ProjectNotFoundError(string name, IDictionary<string, object?>? httpExtensions = null)
        : base($"Project with name '{name}' not found.",  httpExtensions)
    { }
}